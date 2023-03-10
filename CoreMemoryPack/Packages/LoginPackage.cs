using MemoryPack;
using TContentPackage;

namespace CoreMemoryPack;

[MemoryPackable]
public sealed partial class LoginPackage : RpcPackageWithIdentifier
{
    public LoginPackage() : base(CommandKey.Login)
    {
    }

    public string? Username { get; set; }

    public string? Password { get; set; }
}

[MemoryPackable]
public sealed partial class LoginRespPackage : RpcRespPackageWithIdentifier
{
    public LoginRespPackage() : base(CommandKey.LoginAck)
    {
    }
}

