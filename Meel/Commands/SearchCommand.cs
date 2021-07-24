using Meel.Parsing;
using Meel.Responses;
using Meel.Search;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class SearchCommand : IImapCommand
    {
        private IMailStation station;

        public SearchCommand(IMailStation station)
        {
            this.station = station;
        }

        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            if (context.State == SessionState.Selected) {
                // TODO: Implement searching in MailStation
                var searchKey = SearchParser.Parse(requestOptions);
                var list = station.SearchMailbox(context.SelectedMailbox, searchKey);
                response.WriteLine("*", "SEARCH", string.Join(' ', list));
                response.WriteLine(requestId, "OK", "SEARCH completed");
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
