

using ClientMemoryPack;
using CoreMemoryPack;
using SuperSocket.Client.Command;

namespace TContentPackage;

public abstract class OrderCommandAsync<TPacket> : IAsyncCommand<RpcPackageBase> where TPacket : RpcPackageBase
{
    protected abstract ValueTask ExecuteAsync(RpcClient client, TPacket package);

    protected async virtual ValueTask OnSchedulerAsync(RpcClient client, RpcPackageBase package)
    {
        try
        {
            await ExecuteAsync(client, (TPacket)package);
        }
        catch (Exception ex)
        {

        }
    }

    ValueTask IAsyncCommand<RpcPackageBase>.ExecuteAsync(object sender, RpcPackageBase package) => OnSchedulerAsync((RpcClient)sender, package);
}


[Command(Key = (byte)CommandKey.LoginAck)]
public sealed class LoginAck : OrderCommandAsync<LoginRespPackage>
{
    protected override ValueTask ExecuteAsync(RpcClient client, LoginRespPackage package)
    {
        return client.TryDispatchAsync(package);
    }
}
