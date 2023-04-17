using SuperSocket.Client;
using System.Net.Sockets;
using System.Net;
using Microsoft.Extensions.Options;

namespace Client;

public sealed class UnixSocketConnector : ConnectorBase
{
    public IPEndPoint LocalEndPoint { get; private set; }

    public UnixSocketConnector()
    {
    }

    public UnixSocketConnector(IConnector nextConnector)
        : base(nextConnector)
    {
    }

    public UnixSocketConnector(IPEndPoint localEndPoint)
    {
        LocalEndPoint = localEndPoint;
    }

    public UnixSocketConnector(IPEndPoint localEndPoint, IConnector nextConnector)
        : base(nextConnector)
    {
        LocalEndPoint = localEndPoint;
    }

    protected override async ValueTask<ConnectState> ConnectAsync(EndPoint remoteEndPoint, ConnectState state, CancellationToken cancellationToken)
    {
        var socket = new Socket(AddressFamily.Unix, SocketType.Stream, ProtocolType.Unspecified);

        try
        {
            IPEndPoint localEndPoint = LocalEndPoint;
            if (localEndPoint != null)
            {
                socket.ExclusiveAddressUse = false;
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.IpTimeToLive, 1);
                socket.Bind(localEndPoint);
            }

            await socket.ConnectAsync(remoteEndPoint, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ConnectState
            {
                Result = false,
                Exception = exception
            };
        }

        return new ConnectState
        {
            Result = true,
            Socket = socket
        };
    }
}
