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
        private static readonly byte[] missingHint = Encoding.ASCII.GetBytes("No mailbox selected");
        private static readonly byte[] argsHint = Encoding.ASCII.GetBytes("Need to specify a query");
        private static readonly byte[] modeHint =
            Encoding.ASCII.GetBytes("Need to be in Selected mode for this command");
        
        public SearchCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Selected) {
                if (!requestOptions.IsEmpty)
                {
                    if (context.SelectedMailbox != null)
                    {
                        var numMessages = context.SelectedMailbox.NumberOfMessages;
                        var searchKey = SearchKeyParser.Parse(requestOptions, (uint)numMessages);
                        // TODO: Specify UID mode
                        var list = station.SearchMailbox(context.SelectedMailbox, searchKey, true);
                        var listSpan = string.Join(' ', list).AsAsciiSpan();
                        response.Allocate(12 + requestId.Length + completedHint.Length + listSpan.Length + searchHint.Length);
                        response.AppendLine(ImapResponse.Untagged, searchHint, listSpan);
                        response.AppendLine(requestId, ImapResponse.Ok, completedHint);
                    } else
                    {
                        response.Allocate(6 + requestId.Length + missingHint.Length);
                        response.AppendLine(requestId, ImapResponse.No, missingHint);
                    }
                } else
                {
                    response.Allocate(7 + requestId.Length + argsHint.Length);
                    response.AppendLine(requestId, ImapResponse.Bad, argsHint);
                }
            } else
            {
                response.Allocate(7 + requestId.Length + modeHint.Length);
                response.AppendLine(requestId, ImapResponse.Bad, modeHint);
            }
            return 0;
        }
    }
}
