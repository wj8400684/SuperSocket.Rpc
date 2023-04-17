using Microsoft.Extensions.Logging;
using SuperSocket.Channel;
using SuperSocket.ProtoBase;
using SuperSocket.Server;
using SuperSocket;
using System.Net.Sockets;
using SuperSocket.IOCPTcpChannel;
using System.IO.Pipelines;

namespace SuperSocketMemoryPack;

public sealed class TcpIocpChannelCreatorFactory1 : TcpChannelCreatorFactory, IChannelCreatorFactory
{
    public TcpIocpChannelCreatorFactory1(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
    }

    public new IChannelCreator CreateChannelCreator<TPackageInfo>(ListenOptions options, ChannelOptions channelOptions, ILoggerFactory loggerFactory, object pipelineFilterFactory)
    {
        channelOptions.Logger = loggerFactory.CreateLogger("IChannel");

        ILogger channelFactoryLogger = loggerFactory.CreateLogger("TcpChannelCreator");

        var filterFactory = (IPipelineFilterFactory<TPackageInfo>)pipelineFilterFactory;

        return new TcpChannelCreator(options, delegate (Socket s)
        {
            ApplySocketOptions(s, options, channelOptions, channelFactoryLogger);
            return new ValueTask<IChannel>(new IOCPTcpPipeChannel<TPackageInfo>(s, filterFactory.Create(s), channelOptions, new IOQueue()));
        }, channelFactoryLogger);
    }
}
