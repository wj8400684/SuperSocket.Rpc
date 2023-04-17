using Microsoft.AspNetCore.Connections;
using SuperSocket;
using SuperSocket.Channel;

namespace SuperSocketMemoryPack.Kestrel;

public sealed class KestrelChannelCreator : ConnectionHandler, IChannelCreator
{
    private readonly ILogger<KestrelChannelCreator> _logger;

    public ListenOptions Options { get; }

    public bool IsRunning { get; private set; }

    public event NewClientAcceptHandler NewClientAccepted;

    public Func<ConnectionContext, ValueTask<IChannel>> ChannelFactory;

    public KestrelChannelCreator(ILogger<KestrelChannelCreator> logger)
    {
        _logger = logger;
    }

    async Task<IChannel> IChannelCreator.CreateChannel(object connection)
    {
        return await ChannelFactory((ConnectionContext)connection);
    }

    bool IChannelCreator.Start()
    {
        return true;
    }

    Task IChannelCreator.StopAsync()
    {
        return Task.CompletedTask;
    }

    public async override Task OnConnectedAsync(ConnectionContext connection)
    {
        var handler = NewClientAccepted;

        if (handler == null)
            return;

        IChannel channel = null;

        try
        {
            channel = await ChannelFactory(connection);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Failed to create channel for {connection.RemoteEndPoint}.");
            await connection.DisposeAsync();
            return;
        }

        handler.Invoke(this, channel);

        await ((IKestrelPipeChannel)channel).WaitHandleClosingAsync();
    }
}
