using ProtoBuf;
using SuperSocket.ProtoBase;

namespace Core;

[ProtoContract]
public sealed class RpcPackageInfo : IKeyedPackageInfo<CommandKey>
{
    private static readonly Dictionary<Type, CommandKey> CommandTypes = new();

    #region command inilizetion

    internal static void LoadAllCommand()
    {
        var packets = typeof(RpcPackageInfo).Assembly.GetTypes()
            .Where(t => typeof(RpcPackageInfo).IsAssignableFrom(t))
            .Where(t => !t.IsAbstract && t.IsClass)
            .Select(t => (RpcPackageInfo?)Activator.CreateInstance(t));

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

    static RpcPackageInfo()
    {
        LoadAllCommand();
    }

    #endregion

    /// <summary>
    /// 命令
    /// </summary>
    [ProtoMember(1)]
    public CommandKey Key { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [ProtoMember(2)]
    public bool SuccessFul { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [ProtoMember(3)]
    public int ErrorCode { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    [ProtoMember(4)]
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 内容包
    /// </summary>
    [ProtoMember(5)]
    public RpcContentPackage? ContentPackage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [ProtoMember(6)]
    public long Identifier { get; set; }

    public override string ToString()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}

public abstract class RpcContentPackage
{
}
