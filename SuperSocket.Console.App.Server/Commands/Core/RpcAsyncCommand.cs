using Core;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using SuperSocket.Command;
using System.Buffers;

namespace Server.Commands;

public abstract class RpcAsyncCommand<TPackage> :
    IAsyncCommand<RpcSession, RpcPackageInfo>
    where TPackage : class, IMessage<TPackage>
{
    protected readonly MessageParser<TPackage> Parser = new(() => (TPackage)Activator.CreateInstance(typeof(TPackage))!);

    ValueTask IAsyncCommand<RpcSession, RpcPackageInfo>.ExecuteAsync(RpcSession session, RpcPackageInfo package) => SchedulerAsync(session, package);

    protected virtual async ValueTask SchedulerAsync(RpcSession session, RpcPackageInfo package)
    {
        var packet = await DecodePackageAsync(session, package);

        if (packet == null)
            return;

        await ExecuteAsync(session, package, packet);
    }

    protected virtual ValueTask OnHandlerAsync(RpcSession session, RpcPackageInfo packageInfo, TPackage content)
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// 解析包内容
    /// </summary>
    /// <param name="session"></param>
    /// <param name="packet"></param>
    /// <returns></returns>
    protected virtual ValueTask<TPackage> OnDecodePackageAsync(RpcSession session, RpcPackageInfo packet)
    {
        return new ValueTask<TPackage>(Parser.ParseFrom(packet.Content));
    }

    /// <summary>
    /// 解析包内容
    /// </summary>
    /// <param name="session"></param>
    /// <param name="package"></param>
    /// <returns></returns>
    private async ValueTask<TPackage?> DecodePackageAsync(RpcSession session, RpcPackageInfo package)
    {
        try
        {
            return await OnDecodePackageAsync(session, package);
        }
        catch (Exception ex)//未处理异常
        {
            session.LogError(ex, $"[ {session.RemoteAddress} ]-[ {session.SessionID} ]-[ {package.Key} ]- [ {ex.Message} ]未处理异常，内容封包解析错误 ");
        }

        await session.CloseAsync(SuperSocket.Channel.CloseReason.ProtocolError);

        return null;
    }

    /// <summary>
    /// 执行二次封包解析后的内容
    /// </summary>
    /// <param name="session"></param>
    /// <param name="package"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    private async ValueTask ExecuteAsync(RpcSession session, RpcPackageInfo package, TPackage content)
    {
        try
        {
            await OnHandlerAsync(session, package, content);
        }
        catch (TimeoutException ex)
        {
            session.LogError(ex, $"[ {session.RemoteAddress} ]-[ {session.SessionID} ]-[ {ex.Message} ] 任务已经超时");
        }
        catch (Exception ex)//未处理异常 需要断开连接
        {
            session.LogError(ex, $"[ {session.RemoteAddress} ]-[ {session.SessionID} ]-[ {ex.Message} ] 未处理异常，执行二次封包异常。关闭 ");
        }
    }
}

public abstract class RpcAsyncCommand<TPackage, TReplyPackage> : 
    IAsyncCommand<RpcSession, RpcPackageInfo>
    where TPackage : class, IMessage<TPackage>
    where TReplyPackage : class, IMessage<TReplyPackage>
{
    private readonly CommandKey _ack = RpcPackageInfo.GetCommandKey<TReplyPackage>();
    private readonly MessageParser<TPackage> _parser = new(() => (TPackage)Activator.CreateInstance(typeof(TPackage))!);

    ValueTask IAsyncCommand<RpcSession, RpcPackageInfo>.ExecuteAsync(RpcSession session, RpcPackageInfo package) => SchedulerAsync(session, package);

    protected virtual async ValueTask SchedulerAsync(RpcSession session, RpcPackageInfo package)
    {
        var packet = await DecodePackage(session, package);

        if (packet == null)
            return;

        await ExecuteAsync(session, package, packet);
    }

    protected abstract ValueTask<RpcResponse<TReplyPackage>> OnHandlerAsync(RpcSession session, RpcPackageInfo packageInfo, TPackage content);

    /// <summary>
    /// 解析包内容
    /// </summary>
    /// <param name="session"></param>
    /// <param name="packet"></param>
    /// <returns></returns>
    protected virtual ValueTask<TPackage> OnDecodePackageAsync(RpcSession session, RpcPackageInfo packet)
    {
        return new ValueTask<TPackage>(_parser.ParseFrom(packet.Content));
    }

    /// <summary>
    /// 编码包内容
    /// </summary>
    /// <param name="respPacket"></param>
    /// <returns></returns>
    protected virtual ValueTask<ByteString> OnEncodePackageAsync(TReplyPackage respPacket)
    {
        return new ValueTask<ByteString>(respPacket.ToByteString());
    }

    /// <summary>
    /// 解析包内容
    /// </summary>
    /// <param name="session"></param>
    /// <param name="package"></param>
    /// <returns></returns>
    private async ValueTask<TPackage?> DecodePackage(RpcSession session, RpcPackageInfo package)
    {
        try
        {
            return await OnDecodePackageAsync(session, package);
        }
        catch (Exception ex)//未处理异常
        {
            session.LogError(ex, $"[ {session.RemoteAddress} ]-[ {session.SessionID} ]-[ {package.Key} ]- [ {ex.Message} ]未处理异常，内容封包解析错误 ");
        }

        await session.CloseAsync(SuperSocket.Channel.CloseReason.ProtocolError);

        return null;
    }

    /// <summary>
    /// 执行二次封包解析后的内容
    /// </summary>
    /// <param name="session"></param>
    /// <param name="package"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    private async ValueTask ExecuteAsync(RpcSession session, RpcPackageInfo package, TPackage content)
    {
        RpcResponse<TReplyPackage>? response = null;

        var replyPackage = new RpcPackageInfo
        {
            Key = _ack,
            SuccessFul = false,
            Identifier = package.Identifier,
        };

        try
        {
            response = await OnHandlerAsync(session, package, content);
        }
        catch (Exception ex)//未处理异常 需要断开连接
        {
            replyPackage.ErrorMessage = "未知错误,请重试";
            session.LogError(ex, $"[ {session.RemoteAddress} ]-[ {session.SessionID} ]-[ {ex.Message} ] 未处理异常，执行二次封包异常。关闭 ");
        }

        if (response == null)
        {
            await session.SendPacketAsync(replyPackage);
            return;
        }

        try
        {
            replyPackage.Content = await OnEncodePackageAsync(response.Value.Content!);
            replyPackage.SuccessFul = true;
        }
        catch (Exception ex)
        {
            replyPackage.ErrorMessage = "编码错误,请联系管理员";
            session.LogError(ex, $"[ {session.RemoteAddress} ]-[ {session.SessionID} ]-[ {ex.Message} ] 未处理异常，编码内容封包抛出异常");
        }

        await session.SendPacketAsync(replyPackage);
    }
}
