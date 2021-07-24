using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class RenameCommand : IImapCommand
    {
        private IMailStation station;

        public RenameCommand(IMailStation station)
        {
            this.station = station;
        }

        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected) {
                if (!string.IsNullOrEmpty(requestOptions))
                {
                    var parts = requestOptions.Split(' ');
                    if (parts.Length == 2)
                    {
                        var isRenamed = station.RenameMailbox(context.Username, parts[0], parts[1]);
                        if (isRenamed)
                        {
                            response.WriteLine(requestId, "OK", "RENAME completed");
                        }
                        else
                        {
                            response.WriteLine(requestId, "NO", "Cannot rename mailbox with that name");
                        }
                    } else
                    {
                        response.WriteLine(requestId, "BAD", "Need to specify the name to rename to");
                    }
                } else
                {
                    response.WriteLine(requestId, "BAD", "Need to specify the names to rename to and from");
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
