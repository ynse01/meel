using Meel.Parsing;
using Meel.Responses;
using Meel.Search;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class SearchCommand : IImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("SEARCH completed");
        private static readonly byte[] searchHint = Encoding.ASCII.GetBytes("SEARCH");
        private static readonly byte[] argsHint = Encoding.ASCII.GetBytes("Need to specify a query");
        private static readonly byte[] modeHint =
            Encoding.ASCII.GetBytes("Need to be in Selected mode for this command");
        
        private IMailStation station;

        public SearchCommand(IMailStation station)
        {
            this.station = station;
        }

        public int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Selected) {
                if (!requestOptions.IsEmpty)
                {
                    // TODO: Implement searching in MailStation
                    var searchKey = SearchParser.Parse(LexiConstants.AsString(requestOptions));
                    var list = station.SearchMailbox(context.SelectedMailbox, searchKey);
                    response.AppendLine(ImapResponse.Untagged, searchHint, LexiConstants.AsSpan(string.Join(' ', list)));
                    response.AppendLine(requestId, ImapResponse.Ok, completedHint);
                } else
                {
                    response.AppendLine(requestId, ImapResponse.Bad, argsHint);
                }
            } else
            {
                response.AppendLine(requestId, ImapResponse.Bad, modeHint);
            }
            return 0;
        }

        public void ReceiveLiteral(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> literal, ref ImapResponse response)
        {
            // Not applicable
        }
    }
}
