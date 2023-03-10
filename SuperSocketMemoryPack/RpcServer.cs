using Microsoft.Extensions.Options;
using SuperSocket.Server;
using SuperSocket;
using TContentPackage;

namespace SuperSocketMemoryPack;

public sealed class RpcServer : SuperSocketService<RpcPackageBase>
{
    public RpcServer(IServiceProvider serviceProvider, IOptions<ServerOptions> serverOptions) : base(serviceProvider, serverOptions)
    {
    }
}
