using SuperSocket.Command;

namespace Server.Commands;

public sealed class RpcCommand : CommandAttribute
{
    public RpcCommand(CommandKey request)
    {
        Key = (byte)request;
    }
}
