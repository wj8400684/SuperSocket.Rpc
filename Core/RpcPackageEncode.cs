using System.Buffers;
using SuperSocket.ProtoBase;
using System.Text;
using System.Buffers.Binary;

namespace Core;

public sealed class RpcPackageEncode : IPackageEncoder<RpcPackageInfo>
{
    private const byte HeaderSize = sizeof(short);

    public int Encode(IBufferWriter<byte> writer, RpcPackageInfo pack)
    {
        #region 获取头部字节缓冲区

        var headSpan = writer.GetSpan(HeaderSize);
        writer.Advance(HeaderSize);

        #endregion

        #region 写入command

        var length = writer.WriteLittleEndian((byte)pack.Key);
        length += writer.WriteLittleEndian((byte)pack.ForwardKey);

        #endregion

        #region 写入内容

        length += writer.WriteLittleEndian((int)pack.Body.Length);
        foreach (var body in pack.Body)
        {
            length += body.Length;
            writer.Write(body.Span);
        }

        #endregion

        #region 写入状态

        //写入是否成功
        length += writer.WriteLittleEndian(pack.Successful ? (byte)1 : (byte)0);

        #endregion

        #region 写入包id

        length += writer.WriteLittleEndian(pack.Identifier);

        #endregion

        #region 写入错误代码

        length +=  writer.WriteLittleEndian(pack.ErrorCode);

        #endregion

        #region 写入错误消息

        if (!string.IsNullOrWhiteSpace(pack.ErrorMessage))
        {
            length += writer.WriteLittleEndian((byte)pack.ErrorMessage.Length);
            length += writer.Write(pack.ErrorMessage, Encoding.UTF8);
        }

        #endregion

        #region 写入 body 的长度

        BinaryPrimitives.WriteInt16LittleEndian(headSpan, (short)length);

        #endregion

        return length;
    }
}