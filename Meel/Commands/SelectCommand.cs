using Meel.Parsing;
using Meel.Responses;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class SelectCommand : ImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("SELECT completed");
        private static readonly byte[] readWriteHint = Encoding.ASCII.GetBytes("[READ-WRTIE]");
        private static readonly byte[] readOnlyHint = Encoding.ASCII.GetBytes("[READ-ONLY]");
        private static readonly byte[] flagsHint = Encoding.ASCII.GetBytes("FLAGS");
        private static readonly byte[] flagsListHint = 
            Encoding.ASCII.GetBytes(@"\Flagged \Draft \Deleted \Seen \Recent \Answered");
        private static readonly byte[] permFlagsHint = Encoding.ASCII.GetBytes("PERMANENTFLAGS");
        private static readonly byte[] permFlagsListHint =
            Encoding.ASCII.GetBytes(@"\Flagged \Draft \Deleted \Seen \Answered");
        private static readonly byte[] existsHint = Encoding.ASCII.GetBytes("EXISTS");
        private static readonly byte[] recentHint = Encoding.ASCII.GetBytes("RECENT");
        private static readonly byte[] unseenHint = Encoding.ASCII.GetBytes("UNSEEN");
        private static readonly byte[] uidValidityHint = Encoding.ASCII.GetBytes("UIDVALIDITY");
        private static readonly byte[] argsHint =
            Encoding.ASCII.GetBytes("Need to specify a mailbox name");
        private static readonly byte[] authHint =
            Encoding.ASCII.GetBytes("Need to be Authenticated for this command");
        
        public SelectCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected)
            {
                var name = requestOptions.AsString();
                var mailbox = station.SelectMailbox(context.Username, name);
                context.SetSelectedMailbox(mailbox);
                if (!requestOptions.IsEmpty)
                {
                    var lineLength = 20 + permFlagsHint.Length + permFlagsListHint.Length;
                    response.Allocate((6 * lineLength) + requestId.Length + readWriteHint.Length + completedHint.Length);
                    var numMessages = mailbox.NumberOfMessages.AsSpan();
                    var numRecent = mailbox.NumberOfMessages.AsSpan();
                    var firstUnseen = mailbox.FirstUnseenMessage.AsSpan();
                    var mailboxUid = context.SelectedMailbox.Uid;
                    // Flags line
                    response.Append(ImapResponse.Untagged);
                    response.AppendSpace();
                    response.Append(flagsHint);
                    response.AppendSpace();
                    response.Append(LexiConstants.OpenParenthesis);
                    response.Append(flagsListHint);
                    response.Append(LexiConstants.CloseParenthesis);
                    response.AppendLine();
                    // Permanent flags line
                    response.Append(ImapResponse.Untagged);
                    response.AppendSpace();
                    response.Append(LexiConstants.SquareOpenBrace);
                    response.Append(permFlagsHint);
                    response.AppendSpace();
                    response.Append(LexiConstants.OpenParenthesis);
                    response.Append(permFlagsListHint);
                    response.Append(LexiConstants.CloseParenthesis);
                    response.Append(LexiConstants.SquareCloseBrace);
                    response.AppendLine();
                    // Exists line
                    response.AppendLine(ImapResponse.Untagged, numMessages, existsHint);
                    // Recent line
                    response.AppendLine(ImapResponse.Untagged, numRecent, recentHint);
                    // First unseen line
                    response.Append(ImapResponse.Untagged);
                    response.AppendSpace();
                    response.Append(ImapResponse.Ok);
                    response.AppendSpace();
                    response.Append(LexiConstants.SquareOpenBrace);
                    response.Append(unseenHint);
                    response.AppendSpace();
                    response.Append(firstUnseen);
                    response.Append(LexiConstants.SquareCloseBrace);
                    response.AppendLine();
                    // UID Validity line
                    response.Append(ImapResponse.Untagged);
                    response.AppendSpace();
                    response.Append(ImapResponse.Ok);
                    response.AppendSpace();
                    response.Append(LexiConstants.SquareOpenBrace);
                    response.Append(uidValidityHint);
                    response.AppendSpace();
                    response.Append(mailboxUid.AsSpan());
                    response.Append(LexiConstants.SquareOpenBrace);
                    response.AppendLine();
                    ReadOnlySpan<byte> code = (mailbox.CanWrite) ? readWriteHint : readOnlyHint;
                    response.AppendLine(requestId, ImapResponse.Ok, code, completedHint);
                }
                else
                {
                    response.Allocate(7 + requestId.Length + argsHint.Length);
                    response.AppendLine(requestId, ImapResponse.Bad, argsHint);
                }
            }
            else
            {
                response.Allocate(7 + requestId.Length + authHint.Length);
                response.AppendLine(requestId, ImapResponse.Bad, authHint);
            }
            return 0;
        }
    }
}
