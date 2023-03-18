

using Core;
using SuperSocket.Client.Command;

namespace Client;

public abstract class OrderCommandAsync<TPacket> : IAsyncCommand<RpcPackageInfo> where TPacket : RpcContentPackage
{
    protected abstract ValueTask ExecuteAsync(RpcClient client, RpcPackageInfo package, TPacket content);

    protected async virtual ValueTask OnSchedulerAsync(RpcClient client, RpcPackageInfo package)
    {
        var content = package.ContentPackage;

        if (content == null)
            return;
   
        try
        {
            await ExecuteAsync(client, package, (TPacket)content);
        }
        catch (Exception ex)
        {

        }
    }

    ValueTask IAsyncCommand<RpcPackageInfo>.ExecuteAsync(object sender, RpcPackageInfo package) => OnSchedulerAsync((RpcClient)sender, package);
}


[Command(Key = (byte)CommandKey.LoginAck)]
public sealed class LoginAck : OrderCommandAsync<LoginRespPackage>
{
    protected override ValueTask ExecuteAsync(RpcClient client, RpcPackageInfo package, LoginRespPackage content)
    {
        return client.TryDispatchAsync(package);
    }
}
