using Google.Protobuf;
using MQTT.Client;
using SuperSocket.ProtoBase;
using System.Buffers;
using System.Reflection;

namespace Core;

public class CommandTypeInfo
{
    public required CommandKey Command { get; set; }

    public required MessageParser PackageParser { get; set; }

    public IMessage Parser(ReadOnlySequence<byte> body)
    {
        return PackageParser.ParseFrom(body);
    }
}

public class RpcPackageInfo : IKeyedPackageInfo<CommandKey>
{
    private static readonly Dictionary<CommandKey, MessageParser> Commands = new();
    private static readonly Lazy<Dictionary<Type, CommandTypeInfo>> CommandTypes = new(OnLoadCommandTyes);

    static Dictionary<Type, CommandTypeInfo> OnLoadCommandTyes()
    {
        Dictionary<Type, CommandTypeInfo> _map = new();

        foreach (var cmd in Enum.GetValues<CommandKey>())
        {
            var commandType = cmd.GetType();
            var commandName = cmd.ToString();
            var field = commandType.GetField(commandName);

            if (field == null)
                continue;

            var attribute = field.GetCustomAttribute<CommandPackageAttribute>();

            if (attribute == null)
                continue;

            var messageDescriptor = GeneratedCodeReflection.Descriptor.MessageTypes.FirstOrDefault(descriptor => descriptor.ClrType == attribute.PackageType);

            if (messageDescriptor == null)
                continue;

            Commands.TryAdd(cmd, messageDescriptor.Parser);
            _map.TryAdd(attribute.PackageType, new CommandTypeInfo
            {
                Command = cmd,
                PackageParser = messageDescriptor.Parser,
            });
        }

        return _map;
    }

    /// <summary>
    /// 命令
    /// </summary>
    public required CommandKey Key { get; set; }

    /// <summary>
    /// 转发命令
    /// </summary>
    public CommandKey ForwardKey { get; set; }

    /// <summary>
    /// 包内容
    /// </summary>
    public ReadOnlySequence<byte> Body { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Successful { get; set; }

    /// <summary>
    /// 包id
    /// </summary>
    public long Identifier { get; set; }

    /// <summary>
    /// 错误代码
    /// </summary>
    public byte ErrorCode { get; set; }

    /// <summary>
    /// 错误消息
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 解析内容包
    /// </summary>
    /// <typeparam name="TContentPackage"></typeparam>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public TContentPackage DecodeBody<TContentPackage>() where TContentPackage : class, IMessage
    {
        var packageType = typeof(TContentPackage);

        if (!CommandTypes.Value.TryGetValue(packageType, out var info))
            throw new KeyNotFoundException(nameof(packageType));

        return (TContentPackage)info.Parser(Body);
    }

    /// <summary>
    /// 创建 rpc response
    /// </summary>
    /// <typeparam name="TContentPackage"></typeparam>
    /// <returns></returns>
    public RpcResponse<TContentPackage> CreateRpcResponse<TContentPackage>() where TContentPackage : class, IMessage
    {
        return new RpcResponse<TContentPackage>(this);
    }

    /// <summary>
    /// 获取 command key
    /// </summary>
    /// <param name="packageType"></param>
    /// <returns></returns>
    /// <exception cref="KeyNotFoundException"></exception>
    public static CommandKey GetCommandKey(Type packageType)
    {
        if (CommandTypes.Value.TryGetValue(packageType, out var info))
            return info.Command;

        throw new KeyNotFoundException(nameof(packageType));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPackage"></typeparam>
    /// <returns></returns>
    public static CommandKey GetCommandKey<TPackage>()
        where TPackage : class, IMessage
    {
        return GetCommandKey(typeof(TPackage));
    }

    public static RpcPackageInfo Create<TContentPackage>(TContentPackage package) where TContentPackage : class, IMessage
    {
        var body = package.ToByteArray();

        var key = GetCommandKey<TContentPackage>();

        return new RpcPackageInfo
        {
            Key = key,
            Body = new ReadOnlySequence<byte>(body),
        };
    }

    public static RpcPackageInfo Create<TContentPackage>(CommandKey key, TContentPackage package) where TContentPackage : class, IMessage
    {
        var body = package.ToByteArray();

        return new RpcPackageInfo
        {
            Key = key,
            Body = new ReadOnlySequence<byte>(body),
        };
    }

    public static RpcPackageInfo CreateForward<TContentPackage>(TContentPackage package) where TContentPackage : class, IMessage
    {
        var body = package.ToByteArray();

        var key = GetCommandKey<TContentPackage>();

        return new RpcPackageInfo
        {
            ForwardKey = key,
            Key = CommandKey.Forward,
            Body = new ReadOnlySequence<byte>(body),
        };
    }
}
