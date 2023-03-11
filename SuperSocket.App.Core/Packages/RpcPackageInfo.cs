using Google.Protobuf;
using SuperSocket.ProtoBase;
using System.Reflection;

namespace Core;

public sealed class CommandTypeInfo
{
    public required CommandKey Command { get; set; }

    public required MessageParser PackageParser { get; set; }

    public IMessage Parser(ByteString body)
    {
        return PackageParser.ParseFrom(body);
    }
}

public sealed partial class RpcPackageInfo : IKeyedPackageInfo<CommandKey>
{
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

            _map.TryAdd(attribute.PackageType, new CommandTypeInfo
            {
                Command = cmd,
                PackageParser = messageDescriptor.Parser,
            });
        }

        return _map;
    }

    /// <summary>
    /// 
    /// </summary>
    public CommandKey Key
    {
        get => (CommandKey)Command;
        set => Command = (int)value;
    }

    /// <summary>
    /// 转发命令
    /// </summary>
    public CommandKey ForwardKey
    {
        get => (CommandKey)ForwardCommand;
        set => ForwardCommand = (int)value;
    }

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

        return (TContentPackage)info.Parser(Content);
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
        var body = package.ToByteString();

        var key = GetCommandKey<TContentPackage>();

        return new RpcPackageInfo
        {
            Key = key,
            Content = body,
        };
    }

    public static RpcPackageInfo Create<TContentPackage>(CommandKey key, TContentPackage package) where TContentPackage : class, IMessage
    {
        var body = package.ToByteString();

        return new RpcPackageInfo
        {
            Key = key,
            Content = body,
        };
    }

    public static RpcPackageInfo CreateForward<TContentPackage>(TContentPackage package) where TContentPackage : class, IMessage
    {
        var body = package.ToByteString();

        var key = GetCommandKey<TContentPackage>();

        return new RpcPackageInfo
        {
            ForwardKey = key,
            Key = CommandKey.Forward,
            Content = body,
        };
    }
}
