using MQTT.Client;
using Server.Commands;

namespace Server;

[RpcCommand(CommandKey.Login)]
public sealed class Login : RpcAsyncCommand<LoginRequest, LoginReply>
{
    protected override ValueTask<LoginReply> OnHandlerAsync(RpcSession session, RpcPackageInfo packet, LoginRequest content)
    {
        return new ValueTask<LoginReply>(new LoginReply
        {
            UserId = "",
        });
    }
}
