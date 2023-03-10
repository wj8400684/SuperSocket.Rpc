using SuperSocket.Command;

namespace Server.Commands;

[RpcCommand(CommandKey.ForwardAck)]
public sealed class ForwardAck : IAsyncCommand<RpcSession, RpcPackageInfo>
{
    public ValueTask ExecuteAsync(RpcSession session, RpcPackageInfo package)
    {
        return session.TryDispatchAsync(package);
    }
}
