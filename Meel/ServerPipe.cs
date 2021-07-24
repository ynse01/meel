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
        private const byte NewLine = 0xa;

        private ServerSession session;

        public ServerPipe(IRequestResponsePlane plane)
        {
            session = new ServerSession(plane);
        }

        public async Task ProcessAsync(TcpClient client)
        {
            Console.WriteLine($"[{client.Client.RemoteEndPoint}]: connected");
            var reader = PipeReader.Create(client.GetStream());

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
            var position = buffer.PositionOf(NewLine);
            if (position != null)
            {
                // Skip the line and the EndOfLine character.
                line = buffer.Slice(0, position.Value);
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

        private void ProcessLine(in ReadOnlySequence<byte> buffer) {
            var line = GetStringFromSequence(buffer);
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
