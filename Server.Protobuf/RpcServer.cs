using Microsoft.Extensions.Options;
using MQTT.Client;
using SuperSocket.Server;

namespace Server;

internal sealed class RpcServer : SuperSocketService<RpcPackageInfo>
{
    private readonly ISessionContainer _sessionContainer;

    public RpcServer(IServiceProvider serviceProvider, IOptions<ServerOptions> serverOptions)
        : base(serviceProvider, serverOptions)
    {
        _sessionContainer = this.GetSessionContainer();
    }

    internal async ValueTask<RpcResponse<LoginReply>> LoginAsync(LoginRequest request)
    {
        var session = _sessionContainer.GetSessions<RpcSession>().First();

        var resp = await session.GetRpcResponseAsync<LoginRequest, LoginReply>(request);

        return resp;
    }
}