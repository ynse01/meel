using Meel.Parsing;
using Meel.Responses;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class SearchCommand : ImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("SEARCH completed");
        private static readonly byte[] searchHint = Encoding.ASCII.GetBytes("SEARCH");
        private static readonly byte[] argsHint = Encoding.ASCII.GetBytes("Need to specify a query");
        private static readonly byte[] modeHint =
            Encoding.ASCII.GetBytes("Need to be in Selected mode for this command");
        
        public SearchCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Selected) {
                if (!requestOptions.IsEmpty)
                {
                    var numMessages = context.SelectedMailbox.NumberOfMessages;
                    var searchKey = SearchKeyParser.Parse(requestOptions, numMessages);
                    // TODO: Specify UID mode
                    var list = station.SearchMailbox(context.SelectedMailbox, searchKey, true);
                    response.AppendLine(ImapResponse.Untagged, searchHint, string.Join(' ', list).AsAsciiSpan());
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
    }
}
