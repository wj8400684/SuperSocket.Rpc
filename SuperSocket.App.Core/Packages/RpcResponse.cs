﻿using Google.Protobuf;

namespace Core;

public readonly struct RpcResponse<TContentPacket> where TContentPacket : class, IMessage
{
    public RpcResponse(bool successful = false, ErrorCode errorCode = ErrorCode.Null, string? errorMessage = null, TContentPacket? contentPacket = null)
    {
        Successful = successful;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        Content = contentPacket;
    }

    public RpcResponse(RpcPackageInfo package)
    {
        Successful = package.SuccessFul;
        ErrorCode = package.ErrorCode;
        ErrorMessage = package.ErrorMessage;
        Content = package.DecodeBody<TContentPacket>();
    }

    /// <summary>
    /// 包内容
    /// </summary>
    public TContentPacket? Content { get; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public readonly bool Successful { get; }

    /// <summary>
    /// 错误代码
    /// </summary>
    public readonly ErrorCode ErrorCode { get; }

    /// <summary>
    /// 错误消息
    /// </summary>
    public readonly string? ErrorMessage { get; }
}
