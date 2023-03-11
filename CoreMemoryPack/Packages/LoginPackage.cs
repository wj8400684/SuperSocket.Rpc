using MemoryPack;
using MemoryPack.Compression;
using SuperSocket.ProtoBase;
using System.Buffers;
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

//    public override int Encode(IBufferWriter<byte> bufWriter)
//    {
//        using var compressor = new BrotliCompressor();
//        MemoryPackSerializer.Serialize(compressor, this);
//        var compressedBytes = compressor.ToArray();
//        bufWriter.Write(compressedBytes);
//        return compressedBytes.Length;
//    }

//    protected internal override void DecodeBody(ref SequenceReader<byte> reader, object? context)
//    {
//        var refPackage = this;

//        // Decompression(require using)
//        using var decompressor = new BrotliDecompressor();

//        // Get decompressed ReadOnlySequence<byte> from ReadOnlySpan<byte> or ReadOnlySequence<byte>
//        var decompressedBuffer = decompressor.Decompress(reader.UnreadSequence);

//        MemoryPackSerializer.Deserialize(decompressedBuffer, ref refPackage);
//    }
}

[MemoryPackable]
public sealed partial class LoginRespPackage : RpcRespPackageWithIdentifier
{
    public LoginRespPackage() : base(CommandKey.LoginAck)
    {
    }
}

