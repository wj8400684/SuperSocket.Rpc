using Microsoft.Extensions.Hosting;
using SuperSocket.Command;

await SuperSocketHostBuilder.Create<RpcPackageInfo, RpcPipeLineFilter>()
    .UseHostedService<RpcServer>()
    .UseSession<RpcSession>()
    .UseCommand(options => options.AddCommandAssembly(typeof(Login).Assembly))
    .UseClearIdleSession()
    .UseInProcSessionContainer()
    .RunConsoleAsync();
