using SuperSocket.Command;
using SuperSocket.ProtoBase;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.AsSuperSocketHostBuilder<RpcPackageInfo, RpcPipeLineFilter>()
    .UseHostedService<RpcServer>()
    .UseSession<RpcSession>()
    .UseCommand(options => options.AddCommandAssembly(typeof(Login).Assembly))
    .UseClearIdleSession()
    .UseInProcSessionContainer()
    .AsMinimalApiHostBuilder()
    .ConfigureHostBuilder();

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
