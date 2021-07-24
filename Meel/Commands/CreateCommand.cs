using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class CreateCommand : IImapCommand
    {
        private IMailStation station;

        public CreateCommand(IMailStation station)
        {
            this.station = station;
        }

        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected) {
                if (!string.IsNullOrEmpty(requestOptions))
                {
                    var isCreated = station.CreateMailbox(context.Username, requestOptions);
                    if (isCreated)
                    {
                        response.WriteLine(requestId, "OK", "CREATE completed");
                    } else
                    {
                        response.WriteLine(requestId, "NO", "Cannot create mailbox with that name");
                    }
                } else
                {
                    response.WriteLine(requestId, "BAD", "Need to specify the mailbox name to create");
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
