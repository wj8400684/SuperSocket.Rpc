using System.Buffers;
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
        var reader = new SequenceReader<byte>(buffer.Slice(HeaderSize));

        if (!reader.TryRead(out var command))
            throw new ProtocolException($"{command} error");

        var packet = new RpcPackageInfo
        {
            Key = (CommandKey)command,
        };

        #region ForwardKey

        reader.TryRead(out var forwardKey);
        packet.ForwardKey =  (CommandKey)forwardKey;

        #endregion

        #region ¶ÁÈ¡body

        reader.TryReadLittleEndian(out int contentLength);
        var body = reader.UnreadSequence.Slice(0, contentLength);
        packet.Body = body.CopySequence();
        reader.Advance(contentLength);

        #endregion

        #region ×´Ì¬Âë

        reader.TryRead(out var successFul);
        packet.Successful = successFul == 1;

        #endregion

        #region °üid

        reader.TryReadLittleEndian(out long identifier);
        packet.Identifier = identifier;

        #endregion

        #region ´íÎó´úÂë

        reader.TryRead(out var errorCode);
        packet.ErrorCode = errorCode;

        #endregion

        #region ´íÎóÏûÏ¢

        if (reader.TryRead(out var errorMessageLength) && errorMessageLength > 0)
            packet.ErrorMessage = reader.ReadString(Encoding.UTF8, errorMessageLength);

        #endregion

        return packet;
    }

    protected override int GetBodyLengthFromHeader(ref ReadOnlySequence<byte> buffer)
    {
        var reader = new SequenceReader<byte>(buffer);

        reader.TryReadLittleEndian(out short bodyLength);

        return bodyLength;
    }
}