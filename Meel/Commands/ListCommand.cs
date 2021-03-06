using Meel.Parsing;
using Meel.Responses;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Meel.Commands
{
    public class ListCommand : ImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("LIST completed");
        private static readonly byte[] listHint = Encoding.ASCII.GetBytes("LIST");
        private static readonly byte[] cannotHint = Encoding.ASCII.GetBytes("Can't list that reference or name");
        private static readonly byte[] argsHint =
            Encoding.ASCII.GetBytes("Need to specify a mailbox name and a reference");
        private static readonly byte[] authHint =
            Encoding.ASCII.GetBytes("Need to be Authenticated for this command");
        private static readonly byte[] noSelectHint = Encoding.ASCII.GetBytes("\\NoSelect");

        public ListCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected)
            {
                if (!requestOptions.IsEmpty)
                {
                    var index = requestOptions.IndexOf(LexiConstants.Space);
                    if (index >= 0) {
                        var completeList = station.ListMailboxes(context.Username, false);
                        if (completeList.Count > 0)
                        {
                            var padding = 6 + requestId.Length + completedHint.Length;
                            PrepareListResponse(ref response, completeList, padding, false);
                            response.AppendLine(requestId, ImapResponse.Ok, completedHint);
                        } else
                        {
                            response.Allocate(6 + requestId.Length + cannotHint.Length);
                            response.AppendLine(requestId, ImapResponse.No, cannotHint);
                        }
                    } else
                    {
                        response.Allocate(7 + requestId.Length + argsHint.Length);
                        response.AppendLine(requestId, ImapResponse.Bad, argsHint);
                    }
                } else
                {
                    response.Allocate(7 + requestId.Length + argsHint.Length);
                    response.AppendLine(requestId, ImapResponse.Bad, argsHint);
                }
            } else
            {
                response.Allocate(7 + requestId.Length + authHint.Length);
                response.AppendLine(requestId, ImapResponse.Bad, authHint);
            }
            return 0;
        }

        internal static void PrepareListResponse(ref ImapResponse response, List<MailboxInfo> mailboxes, long padding, bool onlySubscribed)
        {
            var linesLength = 0;
            foreach (var item in mailboxes)
            {
                linesLength += (13 + listHint.Length + item.Name.Length);
            }
            response.Allocate((mailboxes.Count * linesLength) + padding);
            foreach (var box in mailboxes)
            {
                if (onlySubscribed && !box.Flags.HasFlag(MailboxFlags.Subscribed))
                {
                    continue;
                }
                response.Append(ImapResponse.Untagged);
                response.AppendSpace();
                response.Append(listHint);
                response.AppendSpace();
                response.Append(LexiConstants.OpenParenthesis);
                PrintMailboxFlags(ref response, box);
                response.Append(LexiConstants.CloseParenthesis);
                response.AppendSpace();
                response.Append(LexiConstants.DoubleQuote);
                response.Append(LexiConstants.DoubleQuote);
                response.AppendSpace();
                response.Append(box.Name);
                response.AppendLine();
            }
        }

        private static void PrintMailboxFlags(ref ImapResponse response, MailboxInfo box)
        {
            if (box.Flags.HasFlag(MailboxFlags.NoSelect))
            {
                response.Append(noSelectHint);
            }
        }
    }
}
