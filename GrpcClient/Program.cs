using Grpc.Net.Client;
using GrpcService;
using System.Diagnostics;

using (var channel = GrpcChannel.ForAddress("http://localhost:5000"))
{
    var client = new Greeter.GreeterClient(channel);

    var watch = new Stopwatch();
    watch.Start();
    Console.WriteLine($"开始执行");
    for (int i = 0; i < 1000000; i++)
    {
        var reply = await client.SayHelloAsync(new HelloRequest
        {
            Name = "afawfaw",
        });
    }

    watch.Stop();
    Console.WriteLine($"执行完成{watch.ElapsedMilliseconds/1000}秒");

    Console.ReadKey();
}
