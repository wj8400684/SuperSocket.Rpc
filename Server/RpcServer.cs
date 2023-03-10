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

    internal async ValueTask<RpcResponse<OrderAddReply>> OrderAddAsync(string sessionId, OrderAddRequest request, CancellationToken cancellationToken)
    {
        var session = _sessionContainer.GetSessionByID(sessionId) ?? throw new Exception("对方已离线");

        return await ((RpcSession)session).GetRpcResponseAsync<OrderAddRequest, OrderAddReply>(request, cancellationToken);
    }
}