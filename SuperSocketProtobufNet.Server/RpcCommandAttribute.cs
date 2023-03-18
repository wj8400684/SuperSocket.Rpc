using SuperSocket.Command;
using Core;

namespace Server;

public sealed class RpcCommandAttribute : CommandAttribute
{
    public RpcCommandAttribute(CommandKey key)
    {
        Key = (byte)key;
    }
}
