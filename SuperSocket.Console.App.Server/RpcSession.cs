using System.Net;
using SuperSocket.ProtoBase;
using SuperSocket.Server;
using SuperSocket.Channel;
using Google.Protobuf;

namespace Server;

public sealed class RpcSession : AppSession
{
    private readonly IPackageEncoder<RpcPackageInfo> _encoder;
    private readonly CancellationTokenSource _connectionTokenSource;
    private readonly PacketDispatcher _packetDispatcher = new();
    private readonly PacketIdentifierProvider _packetIdentifierProvider = new();

    public RpcSession(IPackageEncoder<RpcPackageInfo> encoder)
    {
        _encoder = encoder;
        _connectionTokenSource = new CancellationTokenSource();
    }

    /// <summary>
    /// 远程地址
    /// </summary>
    internal string RemoteAddress { private set; get; } = null!;

    /// <summary>
    /// 客户端连接
    /// </summary>
    /// <returns></returns>
    protected override ValueTask OnSessionConnectedAsync()
    {
        RemoteAddress = ((IPEndPoint)RemoteEndPoint).Address.ToString();

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// 客户端断开
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected override ValueTask OnSessionClosedAsync(CloseEventArgs e)
    {
        _packetIdentifierProvider.Reset();
        _packetDispatcher.CancelAll();
        _packetDispatcher.Dispose();
        _connectionTokenSource.Cancel();
        _connectionTokenSource.Dispose();

        return ValueTask.CompletedTask;
    }


    /// <summary>
    /// 获取响应包
    /// </summary>
    /// <typeparam name="TResponsePacket"></typeparam>
    /// <param name="package"></param>
    /// <param name="responseTimeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    //internal ValueTask<TResponsePacket> GetResponsePacketAsync<TResponsePacket>(
    //     RpcPackage packet,
    //     TimeSpan responseTimeout,
    //     CancellationToken cancellationToken) where TResponsePacket : OrderPacketRespWithIdentifier
    //{
    //    using var timeOut = new CancellationTokenSource(responseTimeout);
    //    return GetResponsePacketAsync<TResponsePacket>(packet, ConnectionToken, cancellationToken, timeOut.Token);
    //}

    /// <summary>
    /// 获取响应包
    /// </summary>
    /// <typeparam name="TResponsePacket"></typeparam>
    /// <param name="packet"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    //internal ValueTask<TResponsePacket> GetResponsePacketAsync<TContentPackage, TResponsePackage>(
    //     TContentPackage packet,
    //     CancellationToken cancellationToken)
    //    where TContentPackage : class, IMessage
    //    where TResponsePackage : class, IMessage
    //{
    //    return GetResponsePacketAsync<TResponsePacket>(packet, ConnectionToken, cancellationToken);
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="package"></param>
    /// <returns></returns>
    internal ValueTask TryDispatchAsync(RpcPackageInfo package)
    {
        var result = _packetDispatcher.TryDispatch(package);

        this.LogDebug($"[{RemoteAddress}-]: commandKey= {package.Key};Identifier= {package.Identifier} TryDispatch result= {result}");

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// 获取响应包
    /// </summary>
    /// <typeparam name="TContentPackage"></typeparam>
    /// <typeparam name="TResponsePackage"></typeparam>
    /// <param name="contentpackage"></param>
    /// <param name="tokens"></param>
    /// <returns></returns>
    internal async ValueTask<RpcResponse<TResponsePackage>> GetRpcResponseAsync<TContentPackage, TResponsePackage>(
        TContentPackage contentpackage,
        params CancellationToken[] tokens)
        where TContentPackage : class, IMessage
        where TResponsePackage : class, IMessage
    {
        var packet = _packetIdentifierProvider.GetNextForwardPacket(contentpackage);

        this.LogDebug($"[{RemoteAddress}]: commandKey= {packet.Key};Identifier= {packet.Identifier} WaitAsync");

        using var tokenSource = CancellationTokenSource.CreateLinkedTokenSource(tokens);
        using var packetAwaitable = _packetDispatcher.AddAwaitable(packet.Identifier);

        try
        {
            await SendPacketAsync(packet);
        }
        catch (Exception e)
        {
            packetAwaitable.Fail(e);
            this.LogError(e, $"[{RemoteAddress}]: commandKey= {packet.Key};Identifier= {packet.Identifier} WaitAsync 发送封包抛出一个异常");
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
                this.LogError($"[{RemoteAddress}]: commandKey= {packet.Key};Identifier= {packet.Identifier} WaitAsync Timeout");

            throw;
        }

        return replyPackage.CreateRpcResponse<TResponsePackage>();
    }

    /// <summary>
    /// 发送包
    /// </summary>
    /// <param name="package"></param>
    /// <returns></returns>
    internal ValueTask SendPacketAsync(RpcPackageInfo package)
    {
        if (Channel.IsClosed)
            return ValueTask.CompletedTask;

        return Channel.SendAsync(_encoder, package);
    }

    /// <summary>
    /// 发送包
    /// </summary>
    /// <typeparam name="TContentPackage"></typeparam>
    /// <param name="content"></param>
    /// <returns></returns>
    internal ValueTask SendPacketAsync<TContentPackage>(TContentPackage content) 
        where TContentPackage : class, IMessage
    {
        if (Channel.IsClosed)
            return ValueTask.CompletedTask;

        var packet = RpcPackageInfo.Create(content);

        return Channel.SendAsync(_encoder, packet);
    }
}
