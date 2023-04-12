using Server.Commands;

namespace Server;

[RpcCommand(CommandKey.Login)]
public sealed class Login : RpcAsyncCommand<LoginRequest, LoginReply>
{
    protected override ValueTask<RpcResponse<LoginReply>> OnHandlerAsync(RpcSession session, RpcPackageInfo packageInfo, LoginRequest content)
    {
        return new ValueTask<RpcResponse<LoginReply>>(new RpcResponse<LoginReply>(
            successful: true,
            contentPacket: new LoginReply
            {
                UserId = Guid.NewGuid().ToString(),
            }));
    }
}
