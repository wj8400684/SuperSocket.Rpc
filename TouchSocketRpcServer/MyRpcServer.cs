using System.ComponentModel;
using TouchCore;
using TouchSocket.Rpc;
using TouchSocket.Rpc.TouchRpc;

namespace TouchSocketRpcServer;

public class MyRpcServer : RpcServer
{
    [Description("登录")]//服务描述，在生成代理时，会变成注释。
    [TouchRpc("Login")]//服务注册的函数键，此处为显式指定。默认不传参的时候，为该函数类全名+方法名的全小写。
    public LoginResponse Login(LoginRequest request)
    {
        return new LoginResponse
        {
            Successful = true,
        };
    }
}