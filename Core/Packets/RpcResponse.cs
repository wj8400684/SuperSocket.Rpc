using Google.Protobuf;

namespace Core;

public readonly struct RpcResponse<TContentPacket> where TContentPacket : class, IMessage
{
    public RpcResponse(RpcPackageInfo packet)
    {
        Successful = packet.Successful;
        ErrorCode = packet.ErrorCode;
        ErrorMessage = packet.ErrorMessage;
        Content = packet.DecodeBody<TContentPacket>();
    }

    /// <summary>
    /// 包内容
    /// </summary>
    public readonly TContentPacket Content { get; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public readonly bool Successful { get; }

    /// <summary>
    /// 错误代码
    /// </summary>
    public readonly byte ErrorCode { get; }

    /// <summary>
    /// 错误消息
    /// </summary>
    public readonly string? ErrorMessage { get; }
}
