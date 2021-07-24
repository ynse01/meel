using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class DeleteCommand : IImapCommand
    {
        private IMailStation station;

        public DeleteCommand(IMailStation station)
        {
            this.station = station;
        }

        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected) {
                if (!string.IsNullOrEmpty(requestOptions))
                {
                    var isDeleted = station.DeleteMailbox(context.Username, requestOptions);
                    if (isDeleted)
                    {
                        response.WriteLine(requestId, "OK", "DELETE completed");
                    } else
                    {
                        response.WriteLine(requestId, "NO", "Cannot delete mailbox with that name");
                    }
                } else
                {
                    response.WriteLine(requestId, "BAD", "Need to specify the mailbox name to delete");
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
