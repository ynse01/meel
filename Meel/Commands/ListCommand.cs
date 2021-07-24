using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class ListCommand : IImapCommand
    {
        private IMailStation station;

        public ListCommand(IMailStation station)
        {
            this.station = station;
        }

        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected)
            {
                if (!string.IsNullOrEmpty(requestOptions))
                {
                    var parts = requestOptions.Split(' ');
                    if (parts.Length == 2) {
                        var completeList = station.ListMailboxes(context.Username, false);
                        if (completeList.Count > 0)
                        {
                            // TODO: Handle subscriptions, references and indicate flags
                            foreach (var name in completeList)
                            {
                                response.WriteLine("*", "LIST", "()", "\"/\"", name);
                            }
                            response.WriteLine(requestId, "OK", "LIST completed");
                        } else
                        {
                            response.WriteLine(requestId, "NO", "Can't list that reference or name");
                        }
                    } else
                    {
                        response.WriteLine(requestId, "BAD", "Need to provide both a reference and a mailbox name");
                    }
                } else
                {
                    response.WriteLine(requestId, "BAD", "Need to provide a reference and mailbox name");
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
