using Meel.Parsing;
using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Meel
{
    public class ServerPipe
    {
        private IRequestResponsePlane plane;
        private ServerSession session;

        public ServerPipe(IRequestResponsePlane plane)
        {
            this.plane = plane;
        }

        public async Task ProcessAsync(TcpClient client)
        {
            Console.WriteLine($"[{client.Client.RemoteEndPoint}]: connected");
            var stream = client.GetStream();
            var reader = PipeReader.Create(stream);
            var writer = PipeWriter.Create(stream);
            session = new ServerSession(plane, writer);

            var isComplete = false;
            while(!isComplete)
            {
                var result = await reader.ReadAsync();
                var buffer = result.Buffer;

                while(TryReadLine(ref buffer, out ReadOnlySequence<byte> line))
                {
                    ProcessLine(line);
                }

                reader.AdvanceTo(buffer.Start, buffer.End);

                isComplete = result.IsCompleted;
            }

            await reader.CompleteAsync();

            Console.WriteLine($"[{client.Client.RemoteEndPoint}]: disconnected");
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

        private void ProcessLine(in ReadOnlySequence<byte> line) {
            session.HandleLine(line);
        }

        private static string GetStringFromSequence(ReadOnlySequence<byte> buffer)
        {
            if (buffer.IsSingleSegment)
            {
                return Encoding.ASCII.GetString(buffer.FirstSpan);
            }
            return string.Create((int)buffer.Length, buffer, (span, sequence) =>
            {
                foreach(var segment in sequence)
                {
                    Encoding.ASCII.GetChars(segment.Span, span);
                    span = span.Slice(segment.Length);
                }
            });
        }
    }
}
