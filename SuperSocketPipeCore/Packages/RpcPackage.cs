using SuperSocket.ProtoBase;
using System.Buffers;
using System.Text;

namespace Core;

public abstract class RpcPackageBase : IKeyedPackageInfo<CommandKey>
{
    protected readonly Type Type;
    private static readonly Dictionary<Type, CommandKey> CommandTypes = new();

    #region command inilizetion

    internal static void LoadAllCommand()
    {
        var packets = typeof(RpcPackageBase).Assembly.GetTypes()
            .Where(t => typeof(RpcPackageBase).IsAssignableFrom(t))
            .Where(t => !t.IsAbstract && t.IsClass)
            .Select(t => (RpcPackageBase?)Activator.CreateInstance(t));

        using var enumerator = packets.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (enumerator.Current != null)
                CommandTypes.TryAdd(enumerator.Current.GetType(), enumerator.Current.Key);
        }
    }

    public static CommandKey GetCommandKey<TPacket>()
    {
        var type = typeof(TPacket);

        if (!CommandTypes.TryGetValue(type, out var key))
            throw new Exception($"{type.Name} δ�̳�PlayPacket");

        return key;
    }

    public static List<KeyValuePair<Type, CommandKey>> GetCommands()
    {
        return CommandTypes.ToList();
    }

    static RpcPackageBase()
    {
        LoadAllCommand();
    }

    #endregion

    protected RpcPackageBase(CommandKey key)
    {
        Key = key;
        Type = GetType();
    }

    /// <summary>
    /// 命令
    /// </summary>
    public CommandKey Key { get; set; }

    public abstract int Encode(IBufferWriter<byte> bufWriter);

    protected internal abstract void DecodeBody(ref SequenceReader<byte> reader, object? context);

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this, Type);
    }
}

public abstract class RpcRespPackage : RpcPackageBase
{
    public string? ErrorMessage { get; set; }

    public bool SuccessFul { get; set; }

    public int ErrorCode { get; set; }

    protected RpcRespPackage(CommandKey key)
        : base(key)
    {
    }
}

public abstract class RpcPackageWithIdentifier : RpcPackageBase
{
    public long Identifier { get; set; }

    protected RpcPackageWithIdentifier(CommandKey key) : base(key)
    {
    }

    public override int Encode(IBufferWriter<byte> bufWriter)
    {
        return bufWriter.WriteLittleEndian(Identifier);
    }

    protected internal override void DecodeBody(ref SequenceReader<byte> reader, object? context)
    {
        reader.TryReadLittleEndian(out long identifier);

        Identifier = identifier;
    }
}

public abstract class RpcRespPackageWithIdentifier : RpcPackageWithIdentifier
{
    protected RpcRespPackageWithIdentifier(CommandKey key) : base(key)
    {
    }

    public string? ErrorMessage { get; set; }

    public bool SuccessFul { get; set; }

    public int ErrorCode { get; set; }

    public override int Encode(IBufferWriter<byte> bufWriter)
    {
        var length = base.Encode(bufWriter);

        length += bufWriter.WriteLittleEndian(SuccessFul);

        if (!string.IsNullOrWhiteSpace(ErrorMessage))
            length += bufWriter.Write(ErrorMessage, Encoding.UTF8);

        length += bufWriter.WriteLittleEndian(ErrorCode);

        return length;
    }

    protected internal override void DecodeBody(ref SequenceReader<byte> reader, object? context)
    {
        base.DecodeBody(ref reader, context);

        reader.TryRead(out var successFul);
        SuccessFul = successFul == 1;

        if (reader.TryRead(out var errorMessageLen) && errorMessageLen > 0)
            ErrorMessage = reader.ReadString(Encoding.UTF8, errorMessageLen);

        reader.TryReadLittleEndian(out int errorCode);
        ErrorCode = errorCode;
    }
}