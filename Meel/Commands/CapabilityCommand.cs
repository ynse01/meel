
using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class CapabilityCommand : IImapCommand
    {
        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            response.WriteLine("*", "CAPABILITY", "IMAP4rev1 LOGINDISABLED");
            response.WriteLine(requestId, "OK", "CAPABILITY completed");
            return response;
        }

        public ImapResponse ReceiveLiteral(ConnectionContext context, string requestId, List<string> literal)
        {
            // Not applicable
            return null;
        }
    }
}
