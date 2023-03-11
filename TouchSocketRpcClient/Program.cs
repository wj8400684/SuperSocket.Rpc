using System.Diagnostics;
using TouchCore;
using TouchSocket.Core;
using TouchSocket.Rpc.TouchRpc;
using TouchSocket.Sockets;

TcpTouchRpcClient client = new TcpTouchRpcClient();
client.Setup(new TouchSocketConfig()
    .SetSerializationSelector(new MemoryPackSerializationSelector())
    .SetRemoteIPHost("127.0.0.1:7789")
    .SetVerifyToken("TouchRpc"));
client.Connect();

var watch = new Stopwatch();
watch.Start();
Console.WriteLine($"开始执行");
for (int i = 0; i < 100000; i++)
{
    //直接调用时，第一个参数为调用键
    //第二个参数为调用配置参数，可设置调用超时时间，取消调用等功能。示例中使用的预设，实际上可以自行new InvokeOption();
    //后续参数为调用参数。
    //泛型为返回值类型。
    var result = client.Invoke<LoginResponse>("Login", InvokeOption.WaitInvoke, new LoginRequest
    {
        Username = "sdaasda",
        Password = "password"
    });

    //   Console.WriteLine($"登录结果：{reply.Successful},响应内容：{reply.Content}");
}

watch.Stop();
Console.WriteLine($"执行完成{watch.ElapsedMilliseconds/1000}秒");

Console.ReadKey();




