using Meel.Parsing;
using Meel.Responses;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class CloseCommand : IImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("CLOSE completed");
        private static readonly byte[] expungeHint = Encoding.ASCII.GetBytes("EXPUNGE");
        private static readonly byte[] wrongHint = Encoding.ASCII.GetBytes("No mailbox by that name");
        private static readonly byte[] modeHint = 
            Encoding.ASCII.GetBytes("Need to be in SELECTED mode for this command");

        private IMailStation station;

        public CloseCommand(IMailStation station)
        {
            this.station = station;
        }

        public int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Selected) {
                var deleted = station.ExpungeBySequence(context.SelectedMailbox);
                if (deleted.Count > 0)
                {
                    var lineLength = 15 + expungeHint.Length;
                    response.Allocate((deleted.Count * lineLength) + 6 + requestId.Length + completedHint.Length);
                    foreach (var id in deleted)
                    {
                        response.AppendLine(ImapResponse.Untagged, LexiConstants.AsSpan(id), expungeHint);
                    }
                    context.SetSelectedMailbox(null);
                    response.AppendLine(requestId, ImapResponse.Ok, completedHint);
                } else
                {
                    response.Allocate(7 + requestId.Length + wrongHint.Length);
                    response.AppendLine(requestId, ImapResponse.No, wrongHint);
                }
            } else
            {
                response.Allocate(7 + requestId.Length + modeHint.Length);
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
