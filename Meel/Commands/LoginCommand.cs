using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class LoginCommand : IImapCommand
    {
        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            if (!string.IsNullOrEmpty(requestOptions))
            {
                var parts = requestOptions.Split(' ', 2);
                if (parts.Length == 2)
                {
                    response.WriteLine(requestId, "OK", "LOGIN accepted");
                    context.Username = parts[0];
                    context.State = SessionState.Authenticated;
                } else
                {
                    response.WriteLine(requestId, "NO", "LOGIN invalid username or password provided");
                    context.State = SessionState.NotAuthenticated;
                }
            } else
            {
                response.WriteLine(requestId, "BAD", "Need to provide username and password");
            }
            return response;
        }

        public ImapResponse ReceiveLiteral(ConnectionContext context, string requestId, List<string> literal)
        {
            // Not applicable
            return null;
        }
    }
}
