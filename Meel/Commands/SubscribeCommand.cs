using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class SubscribeCommand : IImapCommand
    {
        private IMailStation station;

        public SubscribeCommand(IMailStation station)
        {
            this.station = station;
        }

        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected) {
                if (!string.IsNullOrEmpty(requestOptions))
                {
                    var isSubscribed = station.SetSubscription(context.Username, requestOptions, true);
                    if (isSubscribed)
                    {
                        response.WriteLine(requestId, "OK", "SUBSCRIBE completed");
                    } else
                    {
                        response.WriteLine(requestId, "NO", "Cannot subscribe to mailbox with that name");
                    }
                } else
                {
                    response.WriteLine(requestId, "BAD", "Need to specify the mailbox name to subscribe to");
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
