using Google.Protobuf;
using Microsoft.Extensions.Options;
using MQTT.Client;
using SuperSocket.Server;

namespace Server;

public sealed class RpcServer : SuperSocketService<RpcPackageInfo>
{
    private readonly ISessionContainer _sessionContainer;

    public RpcServer(IServiceProvider serviceProvider, IOptions<ServerOptions> serverOptions)
        : base(serviceProvider, serverOptions)
    {
        _sessionContainer = this.GetSessionContainer();
    }


    internal async ValueTask<RpcResponse<TResponsePackage>> GetResponseAsync<TContentPackage, TResponsePackage>(string sessionId, TContentPackage contentpackage, CancellationToken cancellationToken) 
        where TContentPackage : class, IMessage
        where TResponsePackage : class, IMessage
    {
        var session = _sessionContainer.GetSessionByID(sessionId) ?? throw new Exception("对方已离线");

        return await ((RpcSession)session).GetResponseAsync<TContentPackage, TResponsePackage>(contentpackage, cancellationToken);
    }
}