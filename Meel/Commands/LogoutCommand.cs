using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class LogoutCommand : IImapCommand
    {
        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            response.WriteLine("*", "BYE", "MEEL server terminating connection");
            response.WriteLine(requestId, "OK", "LOGOUT completed");
            return response;
        }

        public ImapResponse ReceiveLiteral(ConnectionContext context, string requestId, List<string> literal)
        {
            // Not applicable
            return null;
        }
    }
}
