using SuperSocket.Server;
using SuperSocket;
using Microsoft.Extensions.Logging;
using SuperSocket.Channel;
using SuperSocket.ProtoBase;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using SuperSocket.IOCPTcpChannel;
using System.IO.Pipelines;

namespace Server;

public sealed class TcpUnixChannelCreatorFactory1 : TcpChannelCreatorFactory, IChannelCreatorFactory
{
    public TcpUnixChannelCreatorFactory1(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    public new IChannelCreator CreateChannelCreator<TPackageInfo>(ListenOptions options, ChannelOptions channelOptions, ILoggerFactory loggerFactory, object pipelineFilterFactory)
    {
        var filterFactory = pipelineFilterFactory as IPipelineFilterFactory<TPackageInfo>;
        channelOptions.Logger = loggerFactory.CreateLogger(nameof(IChannel));

        var channelFactoryLogger = loggerFactory.CreateLogger(nameof(TcpChannelCreator));

        return new TcpChannelCreator(options, (s) =>
        {
            ApplySocketOptions(s, options, channelOptions, channelFactoryLogger);
            return new ValueTask<IChannel>((new IOCPTcpPipeChannel<TPackageInfo>(s, filterFactory!.Create(s), channelOptions, PipeScheduler.Inline)));
        }, channelFactoryLogger);
    }
}
