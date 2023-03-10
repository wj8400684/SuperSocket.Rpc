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
        #region ��ȡͷ���ֽڻ�����

        var headSpan = writer.GetSpan(HeaderSize);
        writer.Advance(HeaderSize);

        #endregion

        #region д��command

        var length = writer.WriteLittleEndian((byte)pack.Key);
        length += writer.WriteLittleEndian((byte)pack.ForwardKey);

        #endregion

        #region д������

        length += writer.WriteLittleEndian((int)pack.Body.Length);
        foreach (var body in pack.Body)
        {
            length += body.Length;
            writer.Write(body.Span);
        }

        #endregion

        #region д��״̬

        //д���Ƿ�ɹ�
        length += writer.WriteLittleEndian(pack.Successful ? (byte)1 : (byte)0);

        #endregion

        #region д���id

        length += writer.WriteLittleEndian(pack.Identifier);

        #endregion

        #region д��������

        length +=  writer.WriteLittleEndian(pack.ErrorCode);

        #endregion

        #region д�������Ϣ

        if (!string.IsNullOrWhiteSpace(pack.ErrorMessage))
        {
            length += writer.WriteLittleEndian((byte)pack.ErrorMessage.Length);
            length += writer.Write(pack.ErrorMessage, Encoding.UTF8);
        }

        #endregion

        #region д�� body �ĳ���

        BinaryPrimitives.WriteInt16LittleEndian(headSpan, (short)length);

        #endregion

        return length;
    }
}