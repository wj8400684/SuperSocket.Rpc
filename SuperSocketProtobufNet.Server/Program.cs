﻿using Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server;
using Server.Commands;
using SuperSocket;
using SuperSocket.Command;
using SuperSocket.ProtoBase;

var host = SuperSocketHostBuilder.Create<RpcPackageInfo, RpcPipeLineFilter>()
    .UseHostedService<RpcServer>()
    .UseSession<RpcSession>()
    .UsePackageDecoder<RpcPackageDecoder>()
    .UseCommand(options => options.AddCommandAssembly(typeof(Login).Assembly))
    .UseClearIdleSession()
    .UseInProcSessionContainer()
    .ConfigureServices((context, services) =>
    {
        services.AddLogging();
        services.AddSingleton<IPackageEncoder<RpcPackageInfo>, RpcPackageEncode>();
    })
    .Build();

await host.RunAsync();