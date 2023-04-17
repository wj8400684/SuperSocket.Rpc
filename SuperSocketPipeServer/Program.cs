using Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server;
using Server.Commands;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.IOCPTcpChannelCreatorFactory;
using SuperSocket.ProtoBase;

var host = SuperSocketHostBuilder.Create<RpcPackageBase, RpcPipeLineFilter>()
    .UseHostedService<RpcServer>()
    .UseSession<RpcSession>()
    .UsePackageDecoder<RpcPackageDecoder>()
    .UseCommand(options => options.AddCommandAssembly(typeof(Login).Assembly))
    .UseClearIdleSession()
    .UseInProcSessionContainer()
    .UseChannelCreatorFactory<TcpIocpChannelCreatorFactory1>()
    .ConfigureServices((context, services) =>
    {
        services.AddLogging();
        services.AddSingleton<IPackageEncoder<RpcPackageBase>, RpcPackageEncode>();
    })
    .Build();

await host.RunAsync();