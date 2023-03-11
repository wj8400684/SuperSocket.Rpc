using Core;
using Google.Protobuf;
using SuperSocket.Client.Command;
using System.Buffers;

namespace Client;

public abstract class RpcAsyncCommand<TPackage>
    : IAsyncCommand<RpcPackageInfo>
    where TPackage : class, IMessage<TPackage>
{
    protected readonly MessageParser<TPackage> Parser = new(() => (TPackage)Activator.CreateInstance(typeof(TPackage))!);

    ValueTask IAsyncCommand<RpcPackageInfo>.ExecuteAsync(object sender, RpcPackageInfo package)
    {
        throw new NotImplementedException();
    }

    protected virtual async ValueTask SchedulerAsync(RpcClient client, RpcPackageInfo package)
    {
        var packet = await DecodePackage(client, package);

        if (packet == null)
            return;

        await ExecuteAsync(client, package, packet);
    }

    protected abstract ValueTask OnHandlerAsync(RpcClient client, RpcPackageInfo packageInfo, TPackage content);

    /// <summary>
    /// 解析包内容
    /// </summary>
    /// <param name="client"></param>
    /// <param name="packet"></param>
    /// <returns></returns>
    protected virtual ValueTask<TPackage> OnDecodePackageAsync(RpcClient client, RpcPackageInfo packet)
    {
        return new ValueTask<TPackage>(Parser.ParseFrom(packet.Content));
    }

    /// <summary>
    /// 解析包内容
    /// </summary>
    /// <param name="client"></param>
    /// <param name="package"></param>
    /// <returns></returns>
    private async ValueTask<TPackage?> DecodePackage(RpcClient client, RpcPackageInfo package)
    {
        try
        {
            return await OnDecodePackageAsync(client, package);
        }
        catch (Exception ex)//未处理异常
        {
            //client.LogError(ex, $"[ {client.RemoteAddress} ]-[ {client.SessionID} ]-[ {package.Key} ]- [ {ex.Message} ]未处理异常，内容封包解析错误 ");
        }

        //await client.CloseAsync(SuperSocket.Channel.CloseReason.ProtocolError);

        return null;
    }

    /// <summary>
    /// 执行二次封包解析后的内容
    /// </summary>
    /// <param name="session"></param>
    /// <param name="package"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    private async ValueTask ExecuteAsync(RpcClient session, RpcPackageInfo package, TPackage content)
    {
        try
        {
            await OnHandlerAsync(session, package, content);
        }
        catch (TimeoutException ex)
        {
            //session.LogError(ex, $"[ {session.RemoteAddress} ]-[ {session.SessionID} ]-[ {ex.Message} ] 任务已经超时");
        }
        catch (Exception ex)//未处理异常 需要断开连接
        {
            //session.LogError(ex, $"[ {session.RemoteAddress} ]-[ {session.SessionID} ]-[ {ex.Message} ] 未处理异常，执行二次封包异常。关闭 ");
        }
    }

}

public abstract class ForwardAsyncCommand<TPackage, TReplyPackage> :
    IAsyncCommand<RpcPackageInfo>
    where TPackage : class, IMessage<TPackage>
    where TReplyPackage : class, IMessage<TReplyPackage>
{
    protected readonly MessageParser<TPackage> Parser = new(() => (TPackage)Activator.CreateInstance(typeof(TPackage))!);

    ValueTask IAsyncCommand<RpcPackageInfo>.ExecuteAsync(object sender, RpcPackageInfo package) => SchedulerAsync((RpcClient)sender, package);

    protected virtual async ValueTask SchedulerAsync(RpcClient client, RpcPackageInfo package)
    {
        var packet = await DecodePackage(client, package);

        if (packet == null)
            return;

        await ExecuteAsync(client, package, packet);
    }

    protected abstract ValueTask<TReplyPackage> OnHandlerAsync(RpcClient client, RpcPackageInfo packageInfo, TPackage content);

    /// <summary>
    /// 解析包内容
    /// </summary>
    /// <param name="client"></param>
    /// <param name="packet"></param>
    /// <returns></returns>
    protected virtual ValueTask<TPackage> OnDecodePackageAsync(RpcClient client, RpcPackageInfo packet)
    {
        return new ValueTask<TPackage>(Parser.ParseFrom(packet.Content));
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
    /// <param name="client"></param>
    /// <param name="package"></param>
    /// <returns></returns>
    private async ValueTask<TPackage?> DecodePackage(RpcClient client, RpcPackageInfo package)
    {
        try
        {
            return await OnDecodePackageAsync(client, package);
        }
        catch (Exception ex)//未处理异常
        {
            //client.LogError(ex, $"[ {client.RemoteAddress} ]-[ {client.SessionID} ]-[ {package.Key} ]- [ {ex.Message} ]未处理异常，内容封包解析错误 ");
        }

        //await client.CloseAsync(SuperSocket.Channel.CloseReason.ProtocolError);

        return null;
    }

    /// <summary>
    /// 执行二次封包解析后的内容
    /// </summary>
    /// <param name="session"></param>
    /// <param name="package"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    private async ValueTask ExecuteAsync(RpcClient session, RpcPackageInfo package, TPackage content)
    {
        TReplyPackage? replyContent = null;

        var replyPackage = new RpcPackageInfo
        {
            SuccessFul = true,
            Key = CommandKey.ForwardAck,
            Identifier = package.Identifier,
        };

        try
        {
            replyContent = await OnHandlerAsync(session, package, content);
        }
        catch (TimeoutException ex)
        {
            replyPackage.ErrorMessage = "任务已经超时";
            //session.LogError(ex, $"[ {session.RemoteAddress} ]-[ {session.SessionID} ]-[ {ex.Message} ] 任务已经超时");
        }
        catch (Exception ex)//未处理异常 需要断开连接
        {
            replyPackage.ErrorMessage = "未知错误,请重试";
            //session.LogError(ex, $"[ {session.RemoteAddress} ]-[ {session.SessionID} ]-[ {ex.Message} ] 未处理异常，执行二次封包异常。关闭 ");
        }

        if (replyContent == null)
        {
            await session.SendAsync(replyPackage);
            return;
        }

        try
        {
            replyPackage.Content = await OnEncodePackageAsync(replyContent);
        }
        catch (Exception ex)
        {
            replyPackage.SuccessFul = false;
            replyPackage.ErrorMessage = "编码错误,请联系管理员";
            //session.LogError(ex, $"[ {session.RemoteAddress} ]-[ {session.SessionID} ]-[ {ex.Message} ] 未处理异常，编码内容封包抛出异常");
        }

        await session.SendAsync(replyPackage);
    }
}

[Command(Key = (int)CommandKey.OrderAdd)]
public sealed class OrderAdd : ForwardAsyncCommand<OrderAddRequest, OrderAddReply>
{
    protected override ValueTask<OrderAddReply> OnHandlerAsync(RpcClient client, RpcPackageInfo packageInfo, OrderAddRequest content)
    {
        return new ValueTask<OrderAddReply>(new OrderAddReply
        {
            TargetId = Guid.NewGuid().ToString(),
        });
    }
}

[Command(Key = (int)CommandKey.LoginReply)]
public sealed class LoginAck : IAsyncCommand<RpcPackageInfo>
{
    ValueTask IAsyncCommand<RpcPackageInfo>.ExecuteAsync(object sender, RpcPackageInfo package)
    {
        var client = (RpcClient)sender;

        return client.TryDispatchAsync(package);
    }
}
