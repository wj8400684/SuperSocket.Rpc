using System.Buffers;
using TContentPackageMemoryPack;
using SuperSocket.ProtoBase;

namespace TContentPackage;

/// <summary>
/// | bodyLength | body |
/// | header | cmd | body |
/// </summary>
public sealed class RpcPipeLineFilter : FixedHeaderPipelineFilter<RpcPackageBase>
{
    private const int HeaderSize = sizeof(short);

    public RpcPipeLineFilter()
        : base(HeaderSize)
    {
        PacketFactoryPool.Inilizetion();
    }

    protected override RpcPackageBase DecodePackage(ref ReadOnlySequence<byte> buffer)
    {
        var reader = new SequenceReader<byte>(buffer.Slice(HeaderSize));

        //��ȡ command
        reader.TryRead(out var command);

        var factory = PacketFactoryPool.Get(command);

        if (factory == null)
            throw new ProtocolException($"���{command}δע��");

        return factory.Decode(ref reader);
    }

    protected override int GetBodyLengthFromHeader(ref ReadOnlySequence<byte> buffer)
    {
        var reader = new SequenceReader<byte>(buffer);

        reader.TryReadLittleEndian(out short bodyLength);

        return bodyLength;
    }
}