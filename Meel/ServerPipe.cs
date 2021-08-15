using Meel.Parsing;
using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Meel
{
    public class ServerPipe
    {
        private IMailStation station;
        private ServerSession session;

        public ServerPipe(IMailStation station)
        {
            this.station = station;
        }

        public async Task ProcessAsync(Stream stream)
        {
            var reader = PipeReader.Create(stream);
            session = new ServerSession(stream, station);

            var isComplete = false;
            while (!isComplete)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;

                ProcessBuffer(ref buffer);

                reader.AdvanceTo(buffer.Start, buffer.End);

                isComplete = result.IsCompleted;
            }

            await reader.CompleteAsync();
        }

        public void ProcessAsync(ref ReadOnlySequence<byte> input, Stream output)
        {
            session = new ServerSession(output, station);
            ProcessBuffer(ref input);
        }

        private void ProcessBuffer(ref ReadOnlySequence<byte> buffer)
        {
            while (TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
            {
                session.ProcessLine(line);
            }
        }

        private bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
        {
            bool result;
            var position = buffer.PositionOf(LexiConstants.CarrageReturn);
            if (position != null)
            {
                // Skip the line and the CarrageReturn and EndOfLine characters.
                line = buffer.Slice(0, position.Value);
                var justAfter = buffer.GetPosition(2, position.Value);
                if ((line.Length + 1) < buffer.Length)
                {
                    buffer = buffer.Slice(justAfter);
                } else
                {
                    buffer = ReadOnlySequence<byte>.Empty;
                }
                result = true;
            }
            else
            {
                line = default;
                result = false;
            }
            return result;
        }
    }
}
