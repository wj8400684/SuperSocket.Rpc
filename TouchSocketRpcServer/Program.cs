using TouchCore;
using TouchSocket.Core;
using TouchSocket.Rpc;
using TouchSocket.Rpc.TouchRpc;
using TouchSocket.Sockets;
using TouchSocketRpcServer;

var service = new TcpTouchRpcService();
TouchSocketConfig config = new TouchSocketConfig()//配置
        .SetSerializationSelector(new MemoryPackSerializationSelector())
.SetListenIPHosts(new IPHost[] { new IPHost(7789) })
       .ConfigureRpcStore(a =>
       {
           a.RegisterServer<MyRpcServer>();//注册服务
       })
       .SetVerifyToken("TouchRpc");

service.Setup(config)
    .Start();

service.Logger.Info($"{service.GetType().Name}已启动");

Console.ReadKey();