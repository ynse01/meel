
using Meel.Responses;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class CapabilityCommand : IImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("CAPABILITY completed");
        private static readonly byte[] capabilityHint = 
            Encoding.ASCII.GetBytes("* CAPABILITY IMAP4rev1 LOGINDISABLED");

        public int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> requestOptions, ref ImapResponse response)
        {
            response.Allocate(8 + capabilityHint.Length + completedHint.Length + requestId.Length);
            response.AppendLine(capabilityHint);
            response.AppendLine(requestId, ImapResponse.Ok, completedHint);
            return 0;
        }

        public void ReceiveLiteral(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> literal, ref ImapResponse response)
        {
            // Not applicable
        }
    }
}
