// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: GeneratedBaseCode.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Core {

  /// <summary>Holder for reflection information generated from GeneratedBaseCode.proto</summary>
  public static partial class GeneratedBaseCodeReflection {

    #region Descriptor
    /// <summary>File descriptor for GeneratedBaseCode.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static GeneratedBaseCodeReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChdHZW5lcmF0ZWRCYXNlQ29kZS5wcm90bxIEQ29yZSLIAQoOUnBjUGFja2Fn",
            "ZUluZm8SHQoDa2V5GAEgASgOMhAuQ29yZS5Db21tYW5kS2V5EiQKCkZvcndh",
            "cmRLZXkYAiABKA4yEC5Db3JlLkNvbW1hbmRLZXkSEgoKc3VjY2Vzc0Z1bBgD",
            "IAEoCBIPCgdjb250ZW50GAQgASgMEhIKCmlkZW50aWZpZXIYBSABKAMSFAoM",
            "ZXJyb3JNZXNzYWdlGAYgASgJEiIKCWVycm9yQ29kZRgHIAEoDjIPLkNvcmUu",
            "RXJyb3JDb2RlIh0KCUFkZFBhY2tldBIQCgh1c2VybmFtZRgBIAEoCSraAwoK",
            "Q29tbWFuZEtleRIICgRub25lEAASDQoJaGVhcnRiZWF0EAESEgoOaGVhcnRi",
            "ZWF0UmVwbHkQAhIKCgZyZWZlc2gQAxIPCgtyZWZlc2hSZXBseRAEEgsKB3Jl",
            "c3RhcnQQBRIQCgxyZXN0YXJ0UmVwbHkQBhIJCgVjbG9zZRAHEg4KCmNsb3Nl",
            "UmVwbHkQCBILCgdzdGFydGVkEAkSEAoMc3RhcnRlZFJlcGx5EAoSCgoGc3Rv",
            "cGVkEAsSDwoLc3RvcGVkUmVwbHkQDBIICgRzeW5jEA0SDQoJc3luY1JlcGx5",
            "EA4SBwoDYWRkEA8SDAoIYWRkUmVwbHkQEBIOCgphZGRFeHBUaW1lEBESEwoP",
            "YWRkRXhwVGltZVJlcGx5EBISCQoFbG9naW4QExIOCgpsb2dpblJlcGx5EBQS",
            "DQoJY29ubmVjdGVkEBUSEAoMZGlzY29ubmVjdGVkEBYSEAoMY2xpZW50VXBk",
            "YXRlEBcSFQoRY2xpZW50VXBkYXRlUmVwbHkQGBIQCgxjbGllbnRSZWJvb3QQ",
            "GRIRCg1jbGllbnRSZXN0YXJ0EBoSEwoPY2xpZW50QXV0aG9yaXphEBsSGAoU",
            "Y2xpZW50QXV0aG9yaXphUmVwbHkQHCrZAQoJRXJyb3JDb2RlEggKBG51bGwQ",
            "ABINCgd0aW1lT3V0EKCcARINCgdvZmZsaW5lEKGcARITCg1ub3RQZXJtaXNz",
            "aW9uEKKcARISCgx1bmF1dGhvcml6ZWQQo5wBEhIKDHRva2VuRXhwaXJlZBCk",
            "nAESFQoPd29ya0VudGVyRmFpbGVkEKWcARIUCg53b3JrRXhpdEZhaWxlZBCm",
            "nAESGAoScGFyYW10ZXJDYW5ub3ROdWxsEKecARINCgd0b29GYXN0EKicARIR",
            "CgtkZWNyeXB0RmFpbBCpnAFiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Core.CommandKey), typeof(global::Core.ErrorCode), }, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Core.RpcPackageInfo), global::Core.RpcPackageInfo.Parser, new[]{ "Key", "ForwardKey", "SuccessFul", "Content", "Identifier", "ErrorMessage", "ErrorCode" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Core.AddPacket), global::Core.AddPacket.Parser, new[]{ "Username" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Enums
  public enum CommandKey {
    /// <summary>
    ///未知命令
    /// </summary>
    [pbr::OriginalName("none")] None = 0,
    /// <summary>
    ///心跳命令
    /// </summary>
    [pbr::OriginalName("heartbeat")] Heartbeat = 1,
    /// <summary>
    ///心跳命令
    /// </summary>
    [pbr::OriginalName("heartbeatReply")] HeartbeatReply = 2,
    /// <summary>
    ///刷新订单
    /// </summary>
    [pbr::OriginalName("refesh")] Refesh = 3,
    /// <summary>
    ///刷新订单
    /// </summary>
    [pbr::OriginalName("refeshReply")] RefeshReply = 4,
    /// <summary>
    ///重启订单
    /// </summary>
    [pbr::OriginalName("restart")] Restart = 5,
    /// <summary>
    ///重启订单
    /// </summary>
    [pbr::OriginalName("restartReply")] RestartReply = 6,
    /// <summary>
    ///关闭订单
    /// </summary>
    [pbr::OriginalName("close")] Close = 7,
    /// <summary>
    ///关闭订单
    /// </summary>
    [pbr::OriginalName("closeReply")] CloseReply = 8,
    /// <summary>
    ///订单启动
    /// </summary>
    [pbr::OriginalName("started")] Started = 9,
    /// <summary>
    ///订单启动
    /// </summary>
    [pbr::OriginalName("startedReply")] StartedReply = 10,
    /// <summary>
    ///订单停止
    /// </summary>
    [pbr::OriginalName("stoped")] Stoped = 11,
    /// <summary>
    ///订单停止
    /// </summary>
    [pbr::OriginalName("stopedReply")] StopedReply = 12,
    /// <summary>
    ///同步代理客户端信息
    /// </summary>
    [pbr::OriginalName("sync")] Sync = 13,
    /// <summary>
    ///同步代理客户端信息
    /// </summary>
    [pbr::OriginalName("syncReply")] SyncReply = 14,
    /// <summary>
    ///添加任务
    /// </summary>
    [pbr::OriginalName("add")] Add = 15,
    /// <summary>
    ///添加任务回应
    /// </summary>
    [pbr::OriginalName("addReply")] AddReply = 16,
    /// <summary>
    ///增加任务时间
    /// </summary>
    [pbr::OriginalName("addExpTime")] AddExpTime = 17,
    /// <summary>
    ///增加任务时间
    /// </summary>
    [pbr::OriginalName("addExpTimeReply")] AddExpTimeReply = 18,
    /// <summary>
    ///登录
    /// </summary>
    [pbr::OriginalName("login")] Login = 19,
    /// <summary>
    ///登录
    /// </summary>
    [pbr::OriginalName("loginReply")] LoginReply = 20,
    /// <summary>
    ///客户端连接
    /// </summary>
    [pbr::OriginalName("connected")] Connected = 21,
    /// <summary>
    ///客户端断开连接
    /// </summary>
    [pbr::OriginalName("disconnected")] Disconnected = 22,
    /// <summary>
    ///工作客户端状态更新
    /// </summary>
    [pbr::OriginalName("clientUpdate")] ClientUpdate = 23,
    /// <summary>
    ///工作客户端状态更新回复
    /// </summary>
    [pbr::OriginalName("clientUpdateReply")] ClientUpdateReply = 24,
    /// <summary>
    ///关闭客户端
    /// </summary>
    [pbr::OriginalName("clientReboot")] ClientReboot = 25,
    /// <summary>
    ///重启客户端
    /// </summary>
    [pbr::OriginalName("clientRestart")] ClientRestart = 26,
    /// <summary>
    ///客户端认证
    /// </summary>
    [pbr::OriginalName("clientAuthoriza")] ClientAuthoriza = 27,
    /// <summary>
    ///客户端认证
    /// </summary>
    [pbr::OriginalName("clientAuthorizaReply")] ClientAuthorizaReply = 28,
  }

  /// <summary>
  ///错误代码
  /// </summary>
  public enum ErrorCode {
    [pbr::OriginalName("null")] Null = 0,
    /// <summary>
    ///操作超时
    /// </summary>
    [pbr::OriginalName("timeOut")] TimeOut = 20000,
    /// <summary>
    ///对方已离线
    /// </summary>
    [pbr::OriginalName("offline")] Offline = 20001,
    /// <summary>
    ///不允许操作
    /// </summary>
    [pbr::OriginalName("notPermission")] NotPermission = 20002,
    /// <summary>
    ///客户端未经过授权
    /// </summary>
    [pbr::OriginalName("unauthorized")] Unauthorized = 20003,
    /// <summary>
    ///token过期
    /// </summary>
    [pbr::OriginalName("tokenExpired")] TokenExpired = 20004,
    /// <summary>
    ///工作进入失败
    /// </summary>
    [pbr::OriginalName("workEnterFailed")] WorkEnterFailed = 20005,
    /// <summary>
    ///工作退出失败
    /// </summary>
    [pbr::OriginalName("workExitFailed")] WorkExitFailed = 20006,
    /// <summary>
    ///参数不能为null
    /// </summary>
    [pbr::OriginalName("paramterCannotNull")] ParamterCannotNull = 20007,
    /// <summary>
    ///操作太快
    /// </summary>
    [pbr::OriginalName("tooFast")] TooFast = 20008,
    /// <summary>
    ///解密失败
    /// </summary>
    [pbr::OriginalName("decryptFail")] DecryptFail = 20009,
  }

  #endregion

  #region Messages
  public sealed partial class RpcPackageInfo : pb::IMessage<RpcPackageInfo>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<RpcPackageInfo> _parser = new pb::MessageParser<RpcPackageInfo>(() => new RpcPackageInfo());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<RpcPackageInfo> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Core.GeneratedBaseCodeReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RpcPackageInfo() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RpcPackageInfo(RpcPackageInfo other) : this() {
      key_ = other.key_;
      forwardKey_ = other.forwardKey_;
      successFul_ = other.successFul_;
      content_ = other.content_;
      identifier_ = other.identifier_;
      errorMessage_ = other.errorMessage_;
      errorCode_ = other.errorCode_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RpcPackageInfo Clone() {
      return new RpcPackageInfo(this);
    }

    /// <summary>Field number for the "key" field.</summary>
    public const int KeyFieldNumber = 1;
    private global::Core.CommandKey key_ = global::Core.CommandKey.None;
    /// <summary>
    ///请求或者回复的命令类型
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Core.CommandKey Key {
      get { return key_; }
      set {
        key_ = value;
      }
    }

    /// <summary>Field number for the "ForwardKey" field.</summary>
    public const int ForwardKeyFieldNumber = 2;
    private global::Core.CommandKey forwardKey_ = global::Core.CommandKey.None;
    /// <summary>
    ///转发的key
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Core.CommandKey ForwardKey {
      get { return forwardKey_; }
      set {
        forwardKey_ = value;
      }
    }

    /// <summary>Field number for the "successFul" field.</summary>
    public const int SuccessFulFieldNumber = 3;
    private bool successFul_;
    /// <summary>
    ///回复的的状态
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool SuccessFul {
      get { return successFul_; }
      set {
        successFul_ = value;
      }
    }

    /// <summary>Field number for the "content" field.</summary>
    public const int ContentFieldNumber = 4;
    private pb::ByteString content_ = pb::ByteString.Empty;
    /// <summary>
    ///请求或者回复的数据
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Content {
      get { return content_; }
      set {
        content_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "identifier" field.</summary>
    public const int IdentifierFieldNumber = 5;
    private long identifier_;
    /// <summary>
    ///包id
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long Identifier {
      get { return identifier_; }
      set {
        identifier_ = value;
      }
    }

    /// <summary>Field number for the "errorMessage" field.</summary>
    public const int ErrorMessageFieldNumber = 6;
    private string errorMessage_ = "";
    /// <summary>
    ///回复的错误消息
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ErrorMessage {
      get { return errorMessage_; }
      set {
        errorMessage_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "errorCode" field.</summary>
    public const int ErrorCodeFieldNumber = 7;
    private global::Core.ErrorCode errorCode_ = global::Core.ErrorCode.Null;
    /// <summary>
    ///错误代码
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Core.ErrorCode ErrorCode {
      get { return errorCode_; }
      set {
        errorCode_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as RpcPackageInfo);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(RpcPackageInfo other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Key != other.Key) return false;
      if (ForwardKey != other.ForwardKey) return false;
      if (SuccessFul != other.SuccessFul) return false;
      if (Content != other.Content) return false;
      if (Identifier != other.Identifier) return false;
      if (ErrorMessage != other.ErrorMessage) return false;
      if (ErrorCode != other.ErrorCode) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Key != global::Core.CommandKey.None) hash ^= Key.GetHashCode();
      if (ForwardKey != global::Core.CommandKey.None) hash ^= ForwardKey.GetHashCode();
      if (SuccessFul != false) hash ^= SuccessFul.GetHashCode();
      if (Content.Length != 0) hash ^= Content.GetHashCode();
      if (Identifier != 0L) hash ^= Identifier.GetHashCode();
      if (ErrorMessage.Length != 0) hash ^= ErrorMessage.GetHashCode();
      if (ErrorCode != global::Core.ErrorCode.Null) hash ^= ErrorCode.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Key != global::Core.CommandKey.None) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Key);
      }
      if (ForwardKey != global::Core.CommandKey.None) {
        output.WriteRawTag(16);
        output.WriteEnum((int) ForwardKey);
      }
      if (SuccessFul != false) {
        output.WriteRawTag(24);
        output.WriteBool(SuccessFul);
      }
      if (Content.Length != 0) {
        output.WriteRawTag(34);
        output.WriteBytes(Content);
      }
      if (Identifier != 0L) {
        output.WriteRawTag(40);
        output.WriteInt64(Identifier);
      }
      if (ErrorMessage.Length != 0) {
        output.WriteRawTag(50);
        output.WriteString(ErrorMessage);
      }
      if (ErrorCode != global::Core.ErrorCode.Null) {
        output.WriteRawTag(56);
        output.WriteEnum((int) ErrorCode);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Key != global::Core.CommandKey.None) {
        output.WriteRawTag(8);
        output.WriteEnum((int) Key);
      }
      if (ForwardKey != global::Core.CommandKey.None) {
        output.WriteRawTag(16);
        output.WriteEnum((int) ForwardKey);
      }
      if (SuccessFul != false) {
        output.WriteRawTag(24);
        output.WriteBool(SuccessFul);
      }
      if (Content.Length != 0) {
        output.WriteRawTag(34);
        output.WriteBytes(Content);
      }
      if (Identifier != 0L) {
        output.WriteRawTag(40);
        output.WriteInt64(Identifier);
      }
      if (ErrorMessage.Length != 0) {
        output.WriteRawTag(50);
        output.WriteString(ErrorMessage);
      }
      if (ErrorCode != global::Core.ErrorCode.Null) {
        output.WriteRawTag(56);
        output.WriteEnum((int) ErrorCode);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Key != global::Core.CommandKey.None) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Key);
      }
      if (ForwardKey != global::Core.CommandKey.None) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ForwardKey);
      }
      if (SuccessFul != false) {
        size += 1 + 1;
      }
      if (Content.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Content);
      }
      if (Identifier != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(Identifier);
      }
      if (ErrorMessage.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ErrorMessage);
      }
      if (ErrorCode != global::Core.ErrorCode.Null) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ErrorCode);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(RpcPackageInfo other) {
      if (other == null) {
        return;
      }
      if (other.Key != global::Core.CommandKey.None) {
        Key = other.Key;
      }
      if (other.ForwardKey != global::Core.CommandKey.None) {
        ForwardKey = other.ForwardKey;
      }
      if (other.SuccessFul != false) {
        SuccessFul = other.SuccessFul;
      }
      if (other.Content.Length != 0) {
        Content = other.Content;
      }
      if (other.Identifier != 0L) {
        Identifier = other.Identifier;
      }
      if (other.ErrorMessage.Length != 0) {
        ErrorMessage = other.ErrorMessage;
      }
      if (other.ErrorCode != global::Core.ErrorCode.Null) {
        ErrorCode = other.ErrorCode;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            Key = (global::Core.CommandKey) input.ReadEnum();
            break;
          }
          case 16: {
            ForwardKey = (global::Core.CommandKey) input.ReadEnum();
            break;
          }
          case 24: {
            SuccessFul = input.ReadBool();
            break;
          }
          case 34: {
            Content = input.ReadBytes();
            break;
          }
          case 40: {
            Identifier = input.ReadInt64();
            break;
          }
          case 50: {
            ErrorMessage = input.ReadString();
            break;
          }
          case 56: {
            ErrorCode = (global::Core.ErrorCode) input.ReadEnum();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 8: {
            Key = (global::Core.CommandKey) input.ReadEnum();
            break;
          }
          case 16: {
            ForwardKey = (global::Core.CommandKey) input.ReadEnum();
            break;
          }
          case 24: {
            SuccessFul = input.ReadBool();
            break;
          }
          case 34: {
            Content = input.ReadBytes();
            break;
          }
          case 40: {
            Identifier = input.ReadInt64();
            break;
          }
          case 50: {
            ErrorMessage = input.ReadString();
            break;
          }
          case 56: {
            ErrorCode = (global::Core.ErrorCode) input.ReadEnum();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class AddPacket : pb::IMessage<AddPacket>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<AddPacket> _parser = new pb::MessageParser<AddPacket>(() => new AddPacket());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AddPacket> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Core.GeneratedBaseCodeReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AddPacket() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AddPacket(AddPacket other) : this() {
      username_ = other.username_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AddPacket Clone() {
      return new AddPacket(this);
    }

    /// <summary>Field number for the "username" field.</summary>
    public const int UsernameFieldNumber = 1;
    private string username_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Username {
      get { return username_; }
      set {
        username_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AddPacket);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AddPacket other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Username != other.Username) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Username.Length != 0) hash ^= Username.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (Username.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Username);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (Username.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Username);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Username.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Username);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AddPacket other) {
      if (other == null) {
        return;
      }
      if (other.Username.Length != 0) {
        Username = other.Username;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Username = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            Username = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
