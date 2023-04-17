using KestrelCore;
using MemoryPack;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.IO.Pipelines;
using System.Text;

namespace KestrelServer.Middleware;

internal sealed class RpcConnectionHandler : ConnectionHandler
{
    private readonly ILogger<RpcConnectionHandler> _logger;

    public RpcConnectionHandler(ILogger<RpcConnectionHandler> logger,
                                IServiceProvider serviceProvider)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync(ConnectionContext context)
    {
        var input = context.Transport.Input;
        var output = context.Transport.Output;

        while (context.ConnectionClosed.IsCancellationRequested == false)
        {
            var result = await input.ReadAsync();
            if (result.IsCanceled)
            {
                break;
            }

            if (TryReadEcho(result, out var package, out var consumed))
            {
                WritePackage(output, new LoginRespPackage
                {
                    SuccessFul = true,
                    Identifier = package.Identifier,
                });

                await output.FlushAsync();

                input.AdvanceTo(consumed);
            }
            else
            {
                input.AdvanceTo(result.Buffer.Start, result.Buffer.End);
            }

            if (result.IsCompleted)
                break;
        }
    }


    private const byte HeaderSize = sizeof(short);

    private static void WritePackage(IBufferWriter<byte> writer, LoginRespPackage package)
    {
        #region 获取头部字节缓冲区

        var headSpan = writer.GetSpan(HeaderSize);
        writer.Advance(HeaderSize);

        #endregion

        #region 写入 command

        var length = writer.Write((byte)package.Key);

        #endregion

        #region 写入内容

        length += package.Encode(writer);

        #endregion

        #region 写入 body 的长度

        BinaryPrimitives.WriteInt16LittleEndian(headSpan, (short)length);

        #endregion
    }

    private static bool TryReadEcho(ReadResult result, [MaybeNullWhen(false)] out LoginPackage package, out SequencePosition consumed)
    {
        package = null;

        var reader = new SequenceReader<byte>(result.Buffer);

        //根据头部字节读取body的长度
        if (!reader.TryReadLittleEndian(out short bodyLength))
        {
            consumed = result.Buffer.Start;
            return false;
        }

        if (reader.Remaining < bodyLength)
        {
            consumed = reader.Position;
            return false;
        }

        if (!reader.TryRead(out var command))
        {
            consumed = reader.Position;
            return false;
        }

        var body = reader.UnreadSequence.Slice(0, bodyLength - 1);
        package = MemoryPackSerializer.Deserialize<LoginPackage>(body);
        reader.Advance(bodyLength - 1);
        consumed = reader.Position;

        return package != null;
    }
}
