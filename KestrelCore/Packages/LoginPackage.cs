﻿using MemoryPack;
using System.Buffers;

namespace KestrelCore;

[MemoryPackable]
public sealed partial class LoginPackage : RpcPackageWithIdentifier
{
    public LoginPackage() : base(CommandKey.Login)
    {
    }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public override int Encode(IBufferWriter<byte> bufWriter)
    {
        return base.Encode(bufWriter);
    }

    protected internal override void DecodeBody(ref SequenceReader<byte> reader, object? context)
    {
        base.DecodeBody(ref reader, context);
    }
}

[MemoryPackable]
public sealed partial class LoginRespPackage : RpcRespPackageWithIdentifier
{
    public LoginRespPackage() : base(CommandKey.LoginAck)
    {
    }
}

