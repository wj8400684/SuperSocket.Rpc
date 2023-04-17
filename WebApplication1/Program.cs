using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;
using SuperSocketMemoryPack;
using SuperSocketMemoryPack.Commands;
using TContentPackage;
using SuperSocketMemoryPack.Kestrel;
using Microsoft.AspNetCore.Connections;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    var serverOptions = context.Configuration.GetSection("ServerOptions").Get<ServerOptions>()!;

    foreach (var listeners in serverOptions.Listeners)
    {
        options.Listen(listeners.GetListenEndPoint(), listenOptions =>
        {
            listenOptions.UseConnectionHandler<KestrelChannelCreator>();
        });
    }
});

builder.Host.AsSuperSocketHostBuilder<RpcPackageBase, RpcPipeLineFilter>()
    .UseHostedService<RpcServer>()
    .UseSession<RpcSession>()
    .UsePackageDecoder<RpcPackageDecoder>()
    .UseCommand(options => options.AddCommandAssembly(typeof(Login).Assembly))
    .UseClearIdleSession()
    .UseInProcSessionContainer()
    .UseChannelCreatorFactory<KestrelChannelCreatorFactory>()
    .AsMinimalApiHostBuilder()
    .ConfigureHostBuilder();

builder.Services.AddLogging();
builder.Services.AddSingleton<KestrelChannelCreator>();
builder.Services.AddSingleton<IPackageEncoder<RpcPackageBase>, RpcPackageEncode>();

var app = builder.Build();

await app.RunAsync();

//var host = SuperSocketHostBuilder.Create<RpcPackageBase, RpcPipeLineFilter>()
//    .UseHostedService<RpcServer>()
//    .UseSession<RpcSession>()
//    .UsePackageDecoder<RpcPackageDecoder>()
//    .UseCommand(options => options.AddCommandAssembly(typeof(Login).Assembly))
//    .UseClearIdleSession()
//    .UseInProcSessionContainer()
//    .UseChannelCreatorFactory<TcpUnixChannelCreatorFactory1>()
//    .ConfigureServices((context, services) =>
//    {
//        services.AddLogging();
//        services.AddSingleton<IPackageEncoder<RpcPackageBase>, RpcPackageEncode>();
//    })
//    .Build();

//await host.RunAsync();