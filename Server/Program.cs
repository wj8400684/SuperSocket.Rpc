using Microsoft.AspNetCore.Connections;
using Server.Kestrel;
using SuperSocket.Command;
using SuperSocket.ProtoBase;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel((context, options) =>
{
    var serverOptions = context.Configuration.GetSection("ServerOptions").Get<ServerOptions>();

    foreach (var listeners in serverOptions.Listeners)
    {
        options.Listen(listeners.GetListenEndPoint(), listenOptions =>
        {
            listenOptions.UseConnectionHandler<KestrelChannelCreator>();
        });
    }
});

// Add services to the container.
builder.Host.AsSuperSocketHostBuilder<RpcPackageInfo, RpcPipeLineFilter>()
    .UseHostedService<RpcServer>()
    .UseSession<RpcSession>()
    .UseCommand(options => options.AddCommandAssembly(typeof(Login).Assembly))
    .UseClearIdleSession()
    .UseInProcSessionContainer()
    .UseChannelCreatorFactory<KestrelChannelCreatorFactory>()
    .AsMinimalApiHostBuilder()
    .ConfigureHostBuilder();


builder.Services.AddSingleton<IChannelCreator, KestrelChannelCreator>();
builder.Services.AddSingleton<IPackageEncoder<RpcPackageInfo>, RpcPackageEncode>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
