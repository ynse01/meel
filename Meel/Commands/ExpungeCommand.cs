using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class ExpungeCommand : IImapCommand
    {
        private IMailStation station;

        public ExpungeCommand(IMailStation station)
        {
            this.station = station;
        }

        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            if (context.State == SessionState.Selected) {
                var deleted = station.ExpungeBySequence(context.SelectedMailbox);
                foreach (var id in deleted)
                {
                    response.WriteLine("*", id.ToString(), "EXPUNGE");
                }
                response.WriteLine(requestId, "OK", "EXPUNGE completed");
            } else
            {
                response.WriteLine(requestId, "BAD", "Need to be in SELECTED mode for this command");
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
