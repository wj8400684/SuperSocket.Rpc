using Client;
using Microsoft.Extensions.Logging;
using SuperSocket.Client;
using SuperSocket.Client.Command;
using SuperSocket.ProtoBase;
using System.Net;

namespace Core;



public sealed class RpcClient : EasyCommandClient<CommandKey, RpcPackageBase>
{
    private readonly IEasyClient<RpcPackageBase, RpcPackageBase> _easyClient;
    private readonly PacketDispatcher _packetDispatcher = new();
    private readonly PacketIdentifierProvider _packetIdentifierProvider = new();

    public RpcClient(
        IPackageHandler<CommandKey, RpcPackageBase> packageHandler,
        IPipelineFilter<RpcPackageBase> pipelineFilter,
        IPackageEncoder<RpcPackageBase> packageEncoder,
        ILogger<RpcClient> logger) : base(packageHandler, pipelineFilter, packageEncoder, logger)
    {
        _easyClient = this;
    }

    protected override IConnector GetConnector()
    {
        return new UnixSocketConnector();
    }

    internal new ValueTask<bool> ConnectAsync(EndPoint remoteEndPoint, CancellationToken cancellationToken) => base.ConnectAsync(remoteEndPoint, cancellationToken);

    internal ValueTask<LoginRespPackage> LoginAsync(LoginPackage package, CancellationToken cancellationToken = default)
    {
        return GetResponseAsync<LoginRespPackage>(package, cancellationToken);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="packageInfo"></param>
    /// <returns></returns>
    public ValueTask TryDispatchAsync(RpcRespPackageWithIdentifier packageInfo)
    {
        _packetDispatcher.TryDispatch(packageInfo);

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// 获取响应封包
    /// </summary>
    /// <typeparam name="TRespPacket"></typeparam>
    /// <param name="package"></param>
    /// <exception cref="TimeoutException"></exception>
    /// <exception cref="TaskCanceledException"></exception>
    /// <exception cref="Exception"></exception>
    /// <returns></returns>
    public ValueTask<TRespPacket> GetResponseAsync<TRespPacket>(RpcPackageWithIdentifier package) where TRespPacket : RpcRespPackageWithIdentifier
    {
        return GetResponseAsync<TRespPacket>(package, CancellationToken.None);
    }

    /// <summary>
    /// 获取响应封包
    /// </summary>
    /// <typeparam name="TRespPacket"></typeparam>
    /// <param name="packet"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="TimeoutException"></exception>
    /// <exception cref="TaskCanceledException"></exception>
    /// <exception cref="Exception"></exception>
    /// <returns></returns>
    public async ValueTask<TRespPacket> GetResponseAsync<TRespPacket>(RpcPackageWithIdentifier packet, CancellationToken cancellationToken) where TRespPacket : RpcRespPackageWithIdentifier
    {
        if (CancellationTokenSource == null)
            throw new Exception("没有连接到服务器");

        if (CancellationTokenSource.IsCancellationRequested)
            throw new TaskCanceledException("已经与服务器断开连接");

        cancellationToken.ThrowIfCancellationRequested();

        packet.Identifier = _packetIdentifierProvider.GetNextPacketIdentifier();

        using var packetAwaitable = _packetDispatcher.AddAwaitable<TRespPacket>(packet.Identifier);
        using var cancel = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, CancellationTokenSource!.Token);

        try
        {
            await _easyClient.SendAsync(packet);
        }
        catch (Exception e)
        {
            packetAwaitable.Fail(e);
            throw new Exception("发送封包抛出一个异常", e);
        }

        try
        {
            return await packetAwaitable.WaitAsync(cancel.Token);
        }
        catch (Exception e)
        {
            if (e is TimeoutException)
                throw new TimeoutException($"等待封包调度超时命令：{packet.Key}", e);

            throw new Exception("等待封包调度抛出一个异常", e);
        }
    }
}
