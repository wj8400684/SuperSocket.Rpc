using ProtoBuf;
namespace Core;

[ProtoContract]
public sealed class LoginPackage : RpcContentPackage
{
    [ProtoMember(1)]
    public required string Username { get; set; }

    [ProtoMember(2)]
    public required string Password { get; set; }
}

[ProtoContract]
public sealed class LoginRespPackage : RpcContentPackage
{
}

