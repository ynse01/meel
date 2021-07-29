using Meel.Responses;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class NoopCommand : ImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("NOOP completed");

        public NoopCommand(IMailStation station) : base(station) { }
        
        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> requestOptions, ref ImapResponse response)
        {
            response.Allocate(6 + requestId.Length + completedHint.Length);
            response.AppendLine(requestId, ImapResponse.Ok, completedHint);
            return 0;
        }
    }
}
