
using SuperSocket.ProtoBase;
using System.Buffers;
using System.Text;

namespace Core;

public sealed partial class LoginPackage : RpcPackageWithIdentifier
{
    public LoginPackage()
        : base(CommandKey.Login)
    {
    }

    public required string Username { get; set; }

    public required string Password { get; set; }

    public override int Encode(IBufferWriter<byte> bufWriter)
    {
        var length = base.Encode(bufWriter);

        length += bufWriter.WriteLittleEndian(Username);
        length += bufWriter.WriteLittleEndian(Password);

        return length;
    }

    protected internal override void DecodeBody(ref SequenceReader<byte> reader, object? context)
    {
        base.DecodeBody(ref reader, context);

        reader.TryRead(out var usernameLen);
        Username = reader.ReadString(Encoding.UTF8, usernameLen);

        reader.TryRead(out var passwordfLen);
        Password = reader.ReadString(Encoding.UTF8, passwordfLen);
    }
}

public sealed partial class LoginRespPackage : RpcRespPackageWithIdentifier
{
    public LoginRespPackage() : base(CommandKey.LoginAck)
    {
    }
}

