using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using SuperSocketMemoryPack;
using SuperSocketMemoryPack.Commands;
using TContentPackage;

var host = SuperSocketHostBuilder.Create<RpcPackageBase, RpcPipeLineFilter>()
    .UseHostedService<RpcServer>()
    .UseSession<RpcSession>()
    .UseCommand(options => options.AddCommandAssembly(typeof(Login).Assembly))
    .UseClearIdleSession()
    .UseInProcSessionContainer()
    .ConfigureServices((context, services) =>
    {
        services.AddLogging();
        services.AddSingleton<IPackageEncoder<RpcPackageBase>, RpcPackageEncode>();
    })
    .Build();

await host.RunAsync();