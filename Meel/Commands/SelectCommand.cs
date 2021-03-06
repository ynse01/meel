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
        private static readonly byte[] readWriteHint = Encoding.ASCII.GetBytes("[READ-WRITE]");
        private static readonly byte[] readOnlyHint = Encoding.ASCII.GetBytes("[READ-ONLY]");
        private static readonly byte[] flagsHint = Encoding.ASCII.GetBytes("FLAGS");
        private static readonly byte[] flagsListHint = 
            Encoding.ASCII.GetBytes(@"\Answered \Flagged \Deleted \Seen \Draft");
        private static readonly byte[] permFlagsHint = Encoding.ASCII.GetBytes("PERMANENTFLAGS");
        private static readonly byte[] permFlagsListHint =
            Encoding.ASCII.GetBytes(@"\Answered \Flagged \Deleted \Seen \Draft");
        private static readonly byte[] existsHint = Encoding.ASCII.GetBytes("EXISTS");
        private static readonly byte[] recentHint = Encoding.ASCII.GetBytes("RECENT");
        private static readonly byte[] unseenHint = Encoding.ASCII.GetBytes("UNSEEN");
        private static readonly byte[] uidValidityHint = Encoding.ASCII.GetBytes("UIDVALIDITY");
        private static readonly byte[] unseenMessageHint1 = Encoding.ASCII.GetBytes("Message");
        private static readonly byte[] unseenMessageHint2 = Encoding.ASCII.GetBytes("is the first unseen message");
        private static readonly byte[] uidMessageHint = Encoding.ASCII.GetBytes("Uids valid");
        private static readonly byte[] missingHint =
            Encoding.ASCII.GetBytes("No mailbox by that name");
        private static readonly byte[] argsHint =
            Encoding.ASCII.GetBytes("Need to specify a mailbox name");
        private static readonly byte[] authHint =
            Encoding.ASCII.GetBytes("Need to be Authenticated for this command");
        private const string inbox = "INBOX";

        public SelectCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected)
            {
                if (!requestOptions.IsEmpty) { 
                    var name = requestOptions.AsString();
                    var mailbox = station.SelectMailbox(context.Username, name);
                    if (mailbox != null)
                    {
                        SelectMailboxAndRespond(context, requestId, ref response, mailbox);
                    } else if (string.Compare(name, inbox, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        // Implicitly create INBOX for this (apparently new) user.
                        var isCreated = station.CreateMailbox(context.Username, inbox);
                        if (isCreated)
                        {
                            mailbox = station.SelectMailbox(context.Username, inbox);
                            SelectMailboxAndRespond(context, requestId, ref response, mailbox);
                        } else
                        {
                            response.Allocate(6 + requestId.Length + missingHint.Length);
                            response.AppendLine(requestId, ImapResponse.No, missingHint);
                        }
                    } else
                    {
                        response.Allocate(6 + requestId.Length + missingHint.Length);
                        response.AppendLine(requestId, ImapResponse.No, missingHint);
                    }
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

        internal static void PrepareResponse(ref ImapResponse response, Mailbox mailbox, long padding, bool updateState)
        {
            var lineLength = 20 + permFlagsHint.Length + permFlagsListHint.Length;
            response.Allocate((6 * lineLength) + padding);
            var numMessages = mailbox.NumberOfMessages.AsSpan();
            var numRecent = mailbox.NumberOfRecentMessages.AsSpan();
            var firstUnseen = mailbox.FirstUnseenMessage.AsSpan();
            var mailboxUid = mailbox.Uid;
            // Exists line
            response.AppendLine(ImapResponse.Untagged, numMessages, existsHint);
            // Flags line
            response.Append(ImapResponse.Untagged);
            response.AppendSpace();
            response.Append(flagsHint);
            response.AppendSpace();
            response.Append(LexiConstants.OpenParenthesis);
            response.Append(flagsListHint);
            response.Append(LexiConstants.CloseParenthesis);
            response.AppendLine();
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
            response.AppendSpace();
            response.Append(unseenMessageHint1);
            response.AppendSpace();
            response.Append(firstUnseen);
            response.AppendSpace();
            response.Append(unseenMessageHint2);
            response.AppendLine();
            // Permanent flags line
            response.Append(ImapResponse.Untagged);
            response.AppendSpace();
            response.Append(ImapResponse.Ok);
            response.AppendSpace();
            response.Append(LexiConstants.SquareOpenBrace);
            response.Append(permFlagsHint);
            response.AppendSpace();
            response.Append(LexiConstants.OpenParenthesis);
            response.Append(permFlagsListHint);
            response.Append(LexiConstants.CloseParenthesis);
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
            response.Append(LexiConstants.SquareCloseBrace);
            response.AppendSpace();
            response.Append(uidMessageHint);
            response.AppendLine();
        }

        private void SelectMailboxAndRespond(ConnectionContext context, ReadOnlySequence<byte> requestId, ref ImapResponse response, Mailbox mailbox)
        {
            context.SetSelectedMailbox(mailbox);
            var padding = 7 + requestId.Length + readWriteHint.Length + completedHint.Length;
            PrepareResponse(ref response, mailbox, padding, true);
            ReadOnlySpan<byte> code = (mailbox.CanWrite) ? readWriteHint : readOnlyHint;
            response.AppendLine(requestId, ImapResponse.Ok, code, completedHint);
        }
    }
}
