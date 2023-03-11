using Google.Protobuf;

namespace Core;

public sealed class PacketIdentifierProvider
{
    private readonly object _syncRoot = new();

    private long _value;

    public void Reset()
    {
        lock (_syncRoot)
        {
            _value = 0;
        }

    }

    public long GetNextPacketIdentifier()
    {
        lock (_syncRoot)
        {
            _value++;

            if (_value == 0)
                _value = 1;

            return _value;
        }
    }

    public RpcPackageInfo GetNextPacket<TContentPacket>(TContentPacket content) where TContentPacket : class, IMessage
    {
        var packet = RpcPackageInfo.Create(content);

        packet.Identifier = GetNextPacketIdentifier();

        return packet;
    }

    public RpcPackageInfo GetNextForwardPacket<TContentPacket>(TContentPacket content) where TContentPacket : class, IMessage
    {
        var packet = RpcPackageInfo.CreateForward(content);

        packet.Identifier = GetNextPacketIdentifier();

        return packet;
    }
}