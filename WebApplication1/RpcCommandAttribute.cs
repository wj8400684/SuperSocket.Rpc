using SuperSocket.Command;
using TContentPackage;

namespace SuperSocketMemoryPack;

public sealed class RpcCommandAttribute : CommandAttribute
{
    public RpcCommandAttribute(CommandKey key)
    {
        Key = (byte)key;
    }
}
