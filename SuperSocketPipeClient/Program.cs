using Client;
using Core;
using Microsoft.Extensions.DependencyInjection;
using SuperSocket.Client;
using SuperSocket.Client.Command;
using System.Diagnostics;
using System.Net;

var services = new ServiceCollection();

services.AddLogging();

services.AddCommandClient<CommandKey, RpcPackageBase>(option =>
{
    option.UseClient<RpcClient>();
    option.UsePackageEncoder<RpcPackageEncode>();
    option.UsePipelineFilter<RpcPipeLineFilter>();
    option.UseCommand(options => options.AddCommandAssembly(typeof(LoginAck).Assembly));
});

var provider = services.BuildServiceProvider();

var client = provider.GetRequiredService<RpcClient>();

await client.ConnectAsync(new DnsEndPoint("127.0.0.1", 4040, System.Net.Sockets.AddressFamily.InterNetwork), CancellationToken.None);
var watch = new Stopwatch();
watch.Start();

for (int m = 0; m < 50; m++)
{
    Task.Run(async () =>
    {
        var easyClient = new EasyClient<RpcPackageBase, RpcPackageBase>(new RpcPipeLineFilter(), new RpcPackageEncode()).AsClient();

        await easyClient.ConnectAsync(new DnsEndPoint("127.0.0.1", 4040, System.Net.Sockets.AddressFamily.InterNetwork), CancellationToken.None);

        Console.WriteLine($"开始执行");
        for (int i = 0; i < 1000000; i++)
        {
            //var reply = await client.LoginAsync(new LoginPackage
            //{
            //    Username = "sss",
            //    Password = "password",
            //});

            await easyClient.SendAsync(new LoginPackage
            {
                Username = "sss",
                Password = "password",
            });

            var reply = await easyClient.ReceiveAsync();

            //Console.WriteLine($"登录结果：{reply.Successful},响应内容：{reply.Content}");
        }

    });
}

watch.Stop();
Console.WriteLine($"执行完成{watch.ElapsedMilliseconds/1000}秒");

Console.ReadKey();