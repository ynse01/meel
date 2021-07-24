using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class UnsubscribeCommand : IImapCommand
    {
        private IMailStation station;

        public UnsubscribeCommand(IMailStation station)
        {
            this.station = station;
        }

        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected) {
                if (!string.IsNullOrEmpty(requestOptions))
                {
                    var isSubscribed = station.SetSubscription(context.Username, requestOptions, false);
                    if (isSubscribed)
                    {
                        response.WriteLine(requestId, "OK", "UNSUBSCRIBE completed");
                    } else
                    {
                        response.WriteLine(requestId, "NO", "Cannot unsubscribe to mailbox with that name");
                    }
                } else
                {
                    response.WriteLine(requestId, "BAD", "Need to specify the mailbox name to unsubscribe from");
                }
            } else
            {
                response.WriteLine(requestId, "BAD", "Need to be Authenticated for this command");
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
