using KestrelServer.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var section = builder.Configuration.GetSection("Kestrel");
builder.Services.AddLogging(s=>s.AddConsole());
builder.Host.UseSerilog((hosting, logger) =>
{
    logger.ReadFrom
        .Configuration(hosting.Configuration)
        .Enrich.FromLogContext().WriteTo.Console(outputTemplate: "{Timestamp:O} [{Level:u3}]{NewLine}{SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}");
});

builder.WebHost.UseKestrel(kestrel => kestrel.Configure(section).Endpoint("Rpc", endpoint => endpoint.ListenOptions.UseConnectionHandler<RpcConnectionHandler>()));

var app = builder.Build();

await app.RunAsync();