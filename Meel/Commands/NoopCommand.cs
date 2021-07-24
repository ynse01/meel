using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class NoopCommand : IImapCommand
    {
        public ImapResponse Execute(ConnectionContext context, string requestUid, string requestOptions)
        {
            var response = new ImapResponse();
            response.WriteLine(requestUid, "OK NOOP completed");
            return response;
        }

        public ImapResponse ReceiveLiteral(ConnectionContext context, string requestId, List<string> literal)
        {
            // Not applicable
            return null;
        }
    }
}
