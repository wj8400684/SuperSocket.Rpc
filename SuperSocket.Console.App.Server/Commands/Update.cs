using Server.Commands;

namespace Server;

[RpcCommand(CommandKey.Update)]
public sealed class Update : RpcAsyncCommand<UpdateRequest>
{
    protected override ValueTask OnHandlerAsync(RpcSession session, RpcPackageInfo packageInfo, UpdateRequest content)
    {
        throw new NotImplementedException();
    }
}
