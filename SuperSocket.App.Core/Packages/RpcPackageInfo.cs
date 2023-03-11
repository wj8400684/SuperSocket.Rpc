using Google.Protobuf;
using SuperSocket.ProtoBase;

namespace Core;

public sealed partial class RpcPackageInfo : IKeyedPackageInfo<CommandKey>
{
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

    ///// <summary>
    ///// 获取 command key
    ///// </summary>
    ///// <param name="packageType"></param>
    ///// <returns></returns>
    ///// <exception cref="KeyNotFoundException"></exception>
    //public static CommandKey GetCommandKey(Type packageType)
    //{
    //    if (CommandTypes.Value.TryGetValue(packageType, out var info))
    //        return info.Command;

    //    throw new KeyNotFoundException(nameof(packageType));
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <typeparam name="TPackage"></typeparam>
    ///// <returns></returns>
    //public static CommandKey GetCommandKey<TPackage>()
    //    where TPackage : class, IMessage
    //{
    //    return GetCommandKey(typeof(TPackage));
    //}

    //public static RpcPackageInfo Create<TContentPackage>(TContentPackage package) where TContentPackage : class, IMessage
    //{
    //    var body = package.ToByteArray();

    //    var key = GetCommandKey<TContentPackage>();

    //    return new RpcPackageInfo
    //    {
    //        Key = key,
    //        Body = new ReadOnlySequence<byte>(body),
    //    };
    //}

    //public static RpcPackageInfo Create<TContentPackage>(CommandKey key, TContentPackage package) where TContentPackage : class, IMessage
    //{
    //    var body = package.ToByteArray();

    //    return new RpcPackageInfo
    //    {
    //        Key = key,
    //        Body = new ReadOnlySequence<byte>(body),
    //    };
    //}

    //public static RpcPackageInfo CreateForward<TContentPackage>(TContentPackage package) where TContentPackage : class, IMessage
    //{
    //    var body = package.ToByteArray();

    //    var key = GetCommandKey<TContentPackage>();

    //    return new RpcPackageInfo
    //    {
    //        ForwardKey = key,
    //        Key = CommandKey.Forward,
    //        Body = new ReadOnlySequence<byte>(body),
    //    };
    //}
}
