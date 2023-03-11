using System.Buffers;
using SuperSocket.ProtoBase;

namespace TContentPackage;


/// <summary>
/// | bodyLength | body |
/// | header | cmd | body |
/// </summary>
public sealed class RpcPipeLineFilter : FixedHeaderPipelineFilter<RpcPackageBase>
{
    private const int HeaderSize = sizeof(short);
    private IPacketFactory[] _packetFactories;

    public RpcPipeLineFilter()
        : base(HeaderSize)
    {
        if (_packetFactories != null)
            return;

        var commands = RpcPackageBase.GetCommands();

        _packetFactories = new IPacketFactory[commands.Count + 1];

        foreach (var command in commands)
        {
            var genericType = typeof(DefaultPacketFactory<>).MakeGenericType(command.Key);

            if (Activator.CreateInstance(genericType) is not IPacketFactory packetFactory)
                continue;

            _packetFactories[(int)command.Value] = packetFactory;
        }
    }

    interface IPacketFactory
    {
        RpcPackageBase Create();
    }

    class DefaultPacketFactory<TPacket> : IPacketFactory
        where TPacket : RpcPackageBase, new()
    {
        public RpcPackageBase Create()
        {
            return new TPacket();
        }
    }

    protected override RpcPackageBase DecodePackage(ref ReadOnlySequence<byte> buffer)
    {
        var reader = new SequenceReader<byte>(buffer.Slice(HeaderSize));

        //¶ÁÈ¡ command
        reader.TryRead(out var command);

        var packetFactory = _packetFactories[command];

        if (packetFactory == null)
            throw new ProtocolException($"ÃüÁî£º{command}Î´×¢²á");

        var package = packetFactory.Create();

        package.DecodeBody(ref reader, package);

        return package;
    }

    protected override int GetBodyLengthFromHeader(ref ReadOnlySequence<byte> buffer)
    {
        var reader = new SequenceReader<byte>(buffer);

        reader.TryReadLittleEndian(out short bodyLength);

        return bodyLength;
    }
}