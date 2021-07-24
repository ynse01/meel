using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class BadCommand : IImapCommand
    {
        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            response.WriteLine(requestId, "BAD", "Invalid command syntax or unknown command");
            return response;
        }

        public ImapResponse ReceiveLiteral(ConnectionContext context, string requestId, List<string> literal)
        {
            // Not applicable
            return null;
        }
    }
}
