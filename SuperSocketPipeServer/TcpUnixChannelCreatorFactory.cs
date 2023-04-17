using SuperSocket.Server;
using SuperSocket;
using Microsoft.Extensions.Logging;
using SuperSocket.Channel;
using SuperSocket.ProtoBase;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using SuperSocket.IOCPTcpChannel;

namespace Server;

public sealed class TcpIocpChannelCreatorFactory : TcpChannelCreatorFactory, IChannelCreatorFactory
{
    public TcpIocpChannelCreatorFactory(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public new IChannelCreator CreateChannelCreator<TPackageInfo>(ListenOptions options, ChannelOptions channelOptions, ILoggerFactory loggerFactory, object pipelineFilterFactory)
    {
        var filterFactory = pipelineFilterFactory as IPipelineFilterFactory<TPackageInfo>;
        channelOptions.Logger = loggerFactory.CreateLogger(nameof(IChannel));

        var channelFactoryLogger = loggerFactory.CreateLogger(nameof(TcpUnixChannelCreator));

        return new TcpChannelCreator(options, (s) =>
        {
            ApplySocketOptions(s, options, channelOptions, channelFactoryLogger);
            return new ValueTask<IChannel>((new IOCPTcpPipeChannel<TPackageInfo>(s, filterFactory!.Create(s), channelOptions)));
        }, channelFactoryLogger);
    }
}
