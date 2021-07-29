
using Meel.Responses;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class CapabilityCommand : ImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("CAPABILITY completed");
        private static readonly byte[] capabilityHint = 
            Encoding.ASCII.GetBytes("* CAPABILITY IMAP4rev1 LOGINDISABLED");

        public CapabilityCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> requestOptions, ref ImapResponse response)
        {
            response.Allocate(8 + capabilityHint.Length + completedHint.Length + requestId.Length);
            response.AppendLine(capabilityHint);
            response.AppendLine(requestId, ImapResponse.Ok, completedHint);
            return 0;
        }
    }
}
