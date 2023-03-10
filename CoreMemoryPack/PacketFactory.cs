using TContentPackage;
using MemoryPack;
using SuperSocket.ProtoBase;
using System.Buffers;

namespace TContentPackageMemoryPack;

internal class PacketFactory<TPacket> : IPacketFactory
    where TPacket : RpcPackageBase
{
    public RpcPackageBase Decode(ref SequenceReader<byte> reader)
    {
        var result = MemoryPackSerializer.Deserialize<TPacket>(reader.UnreadSequence);

        if (result == null)
            throw new ProtocolException($"反序列化数据失败！字节长度：{reader.UnreadSequence.Length}");

        return result;
    }
}

