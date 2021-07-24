using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class SelectCommand : IImapCommand
    {
        private IMailStation station;

        public SelectCommand(IMailStation station)
        {
            this.station = station;
        }

        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected) {
                var mailbox = station.SelectMailbox(context.Username, requestOptions);
                if (!string.IsNullOrEmpty(requestOptions))
                {
                    response.WriteLine("*", mailbox.NumberOfMessages.ToString(), "EXISTS");
                    response.WriteLine("*", mailbox.NumberOfRecentMessages.ToString(), "RECENT");
                    response.WriteLine("*", "OK", $"[UNSEEN {mailbox.FirstUnseenMessage}]");
                    // TODO: Write UID, UIDVALIDITY, FLAGS and PERMANENTFLAGS responses also
                    var code = (mailbox.CanWrite) ? "[READ-WRITE]" : "[READ-ONLY]";
                    response.WriteLine(requestId, "OK", code, "SELECT completed");
                    context.SetSelectedMailbox(mailbox);
                } else
                {
                    response.WriteLine(requestId, "BAD", "Need to provide a mailbox name");
                    context.SetSelectedMailbox(null);
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
