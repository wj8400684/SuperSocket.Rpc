using Core;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using SuperSocket.Client;
using SuperSocket.Client.Command;
using SuperSocket.ProtoBase;
using System.Net;

namespace Client;

public sealed class RpcClient : EasyCommandClient<CommandKey, RpcPackageInfo>
{
    private readonly IEasyClient<RpcPackageInfo, RpcPackageInfo> _easyClient;
    private readonly PacketDispatcher _packetDispatcher = new();
    private readonly PacketIdentifierProvider _packetIdentifierProvider = new();

    public RpcClient(
        IPackageHandler<CommandKey, RpcPackageInfo> packageHandler,
        IPipelineFilter<RpcPackageInfo> pipelineFilter,
        IPackageEncoder<RpcPackageInfo> packageEncoder,
        ILogger<RpcClient> logger) : base(packageHandler, pipelineFilter, packageEncoder, logger)
    {
        _easyClient = this;
    }

    internal new ValueTask<bool> ConnectAsync(EndPoint remoteEndPoint, CancellationToken cancellationToken) => base.ConnectAsync(remoteEndPoint, cancellationToken);

    internal ValueTask<RpcResponse<LoginReply>> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        return GetRpcResponseAsync<LoginRequest, LoginReply>(request, cancellationToken);
    }

    protected async override ValueTask OnPackageHandlerAsync(EasyClient<RpcPackageInfo> sender, RpcPackageInfo package)
    {
        try
        {
            if (package.ForwardKey == CommandKey.None)
                await base.OnPackageHandlerAsync(sender, package);
            else
                await PackageCommandHandler.HandleAsync(this, package, package.ForwardKey);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, $"命令：{package}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="packageInfo"></param>
    /// <returns></returns>
    public ValueTask TryDispatchAsync(RpcPackageInfo packageInfo)
    {
        _packetDispatcher.TryDispatch(packageInfo);

        return ValueTask.CompletedTask;
    }

    internal async ValueTask<RpcResponse<TResponsePackage>> GetRpcResponseAsync<TContentPackage, TResponsePackage>(
        TContentPackage contentpackage,
        params CancellationToken[] tokens)
        where TContentPackage : class, IMessage
        where TResponsePackage : class, IMessage
    {
        if (CancellationTokenSource == null)
            throw new Exception("没有连接到服务器");

        if (CancellationTokenSource.IsCancellationRequested)
            throw new TaskCanceledException("已经与服务器断开连接");

        var package = _packetIdentifierProvider.GetNextPacket(contentpackage);

        Logger.LogDebug($"commandKey= {package.Key};Identifier= {package.Identifier} WaitAsync");

        using var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(tokens);
        using var packetAwaitable = _packetDispatcher.AddAwaitable(package.Identifier);

        try
        {
            await _easyClient.SendAsync(package);
        }
        catch (Exception e)
        {
            packetAwaitable.Fail(e);
            Logger.LogError(e, $"commandKey= {package.Key};Identifier= {package.Identifier} WaitAsync 发送封包抛出一个异常");
        }

        RpcPackageInfo replyPackage;

        try
        {
            //等待封包结果
            replyPackage = await packetAwaitable.WaitAsync(tokenSource.Token);
        }
        catch (Exception e)
        {
            if (e is TimeoutException)
                Logger.LogError($"commandKey= {package.Key};Identifier= {package.Identifier} WaitAsync Timeout");

            throw;
        }

        return replyPackage.CreateRpcResponse<TResponsePackage>();
    }
}
