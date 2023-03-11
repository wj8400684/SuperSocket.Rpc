using System.Buffers;
using SuperSocket.ProtoBase;
using Google.Protobuf;

namespace Core;

public sealed class RpcPackageEncode : IPackageEncoder<RpcPackageInfo>
{
    public int Encode(IBufferWriter<byte> writer, RpcPackageInfo pack)
    {
        var bodyLength = pack.CalculateSize();

        writer.WriteLittleEndian((short)bodyLength);

        pack.WriteTo(writer);

        return bodyLength;
    }
}