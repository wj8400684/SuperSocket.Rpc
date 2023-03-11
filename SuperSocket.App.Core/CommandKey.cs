
namespace Core;

public enum CommandKey : int
{
    None,

    [CommandPackage(typeof(LoginRequest))] Login,

    [CommandPackage(typeof(LoginReply))] LoginReply,

    [CommandPackage(typeof(UpdateRequest))] Update,

    [CommandPackage(typeof(OrderAddRequest))] OrderAdd,

    [CommandPackage(typeof(OrderAddReply))] OrderAddReply,

    [CommandPackage(typeof(OrderCloseRequest))] OrderClose,

    [CommandPackage(typeof(OrderCloseReply))] OrderCloseReply,

    [CommandPackage(typeof(OrderCloseReply))] Forward,

    [CommandPackage(typeof(OrderCloseReply))] ForwardAck,
}
