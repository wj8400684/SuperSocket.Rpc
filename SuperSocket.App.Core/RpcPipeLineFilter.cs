using System.Buffers;
using System.Net.Http.Headers;
using System.Text;
using SuperSocket.ProtoBase;

namespace Core;

/// <summary>
/// | bodyLength | body |
/// | header | cmd | body |
/// </summary>
public sealed class RpcPipeLineFilter : FixedHeaderPipelineFilter<RpcPackageInfo>
{
    private const int HeaderSize = sizeof(short);

    public RpcPipeLineFilter()
        : base(HeaderSize)
    {
    }

    protected override RpcPackageInfo DecodePackage(ref ReadOnlySequence<byte> buffer)
    {
        return RpcPackageInfo.Parser.ParseFrom(buffer.Slice(HeaderSize));
    }

    protected override int GetBodyLengthFromHeader(ref ReadOnlySequence<byte> buffer)
    {
        var reader = new SequenceReader<byte>(buffer);

        reader.TryReadLittleEndian(out short bodyLength);

        return bodyLength;
    }
}