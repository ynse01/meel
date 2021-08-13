using Meel.Parsing;
using System;
using System.Buffers;
using System.IO;
using System.IO.Pipelines;
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

        public async Task ProcessAsync(Stream input, Stream output)
        {
            var reader = PipeReader.Create(input);
            session = new ServerSession(output, station);

            var isComplete = false;
            while (!isComplete)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;

                while (TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
                {
                    session.ProcessLine(line);
                }

                reader.AdvanceTo(buffer.Start, buffer.End);

                isComplete = result.IsCompleted;
            }

            await reader.CompleteAsync();
        }

        private bool TryReadLine(ref ReadOnlySequence<byte> buffer, out ReadOnlySequence<byte> line)
        {
            bool result;
            var position = buffer.PositionOf(LexiConstants.NewLine);
            if (position != null)
            {
                // Skip the line and the CarrageReturn and EndOfLine characters.
                line = buffer.Slice(0, buffer.GetPosition(-1, position.Value));
                buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
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
