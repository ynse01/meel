﻿using Meel.Parsing;
using Meel.Responses;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class SelectCommand : IImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("SELECT completed");
        private static readonly byte[] readWriteHint = Encoding.ASCII.GetBytes("[READ-WRTIE]");
        private static readonly byte[] readOnlyHint = Encoding.ASCII.GetBytes("[READ-ONLY]");
        private static readonly byte[] existsHint = Encoding.ASCII.GetBytes("EXISTS");
        private static readonly byte[] recentHint = Encoding.ASCII.GetBytes("RECENT");
        private static readonly byte[] unseenHint = Encoding.ASCII.GetBytes("UNSEEN");
        private static readonly byte[] argsHint =
            Encoding.ASCII.GetBytes("Need to specify a mailbox name");
        private static readonly byte[] authHint =
            Encoding.ASCII.GetBytes("Need to be Authenticated for this command");
        
        private IMailStation station;

        public SelectCommand(IMailStation station)
        {
            this.station = station;
        }

        public int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected)
            {
                var name = LexiConstants.AsString(requestOptions);
                var mailbox = station.SelectMailbox(context.Username, name);
                context.SetSelectedMailbox(mailbox);
                if (!requestOptions.IsEmpty)
                {
                    var lineLength = 10 + existsHint.Length;
                    response.Allocate((3 * lineLength) + requestId.Length + readWriteHint.Length + completedHint.Length);
                    var numMessages = LexiConstants.AsSpan(mailbox.NumberOfMessages);
                    var numRecent = LexiConstants.AsSpan(mailbox.NumberOfMessages);
                    var firstUnseen = LexiConstants.AsSpan(mailbox.FirstUnseenMessage);
                    response.AppendLine(ImapResponse.Untagged, numMessages, existsHint);
                    response.AppendLine(ImapResponse.Untagged, numRecent, recentHint);
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
                    // TODO: Write UID, UIDVALIDITY, FLAGS and PERMANENTFLAGS responses also
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

        public void ReceiveLiteral(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> literal, ref ImapResponse response)
        {
            // Not applicable
        }
    }
}
