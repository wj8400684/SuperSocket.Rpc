using TContentPackage;
using System.Buffers;

namespace TContentPackageMemoryPack;

internal interface IPacketFactory
{
    RpcPackageBase Decode(ref SequenceReader<byte> body);
}


internal class PacketFactoryPool
{
    private static IPacketFactory[]? _packetFactorys;

    public static void Inilizetion()
    {
        if (_packetFactorys != null)
            return;

        var commands = RpcPackageBase.GetCommands();

        _packetFactorys = new IPacketFactory[commands.Count + 1];

        foreach (var command in commands)
        {
            var genericType = typeof(PacketFactory<>).MakeGenericType(command.Key);

            if (Activator.CreateInstance(genericType) is not IPacketFactory packetFactory)
                continue;

            _packetFactorys[(int)command.Value] = packetFactory;
        }
    }

    public static IPacketFactory? Get(byte key)
    {
        ArgumentNullException.ThrowIfNull(_packetFactorys);

        return _packetFactorys[key];
    }
}