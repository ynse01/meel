﻿using Meel.Parsing;
using Meel.Responses;
using MimeKit;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class StoreCommand : IImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("STORE completed");
        private static readonly byte[] noneHint = Encoding.ASCII.GetBytes("No messages found");
        private static readonly byte[] argsHint = 
            Encoding.ASCII.GetBytes("Need to specify a sequence number and item name");
        private static readonly byte[] modeHint =
            Encoding.ASCII.GetBytes("Need to be in Selected mode for this command");

        private IMailStation station;

        public StoreCommand(IMailStation station)
        {
            this.station = station;
        }

        public int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Selected) {
                var index = requestOptions.PositionOf(LexiConstants.Space);
                if (index.HasValue)
                {
                    var mailbox = context.SelectedMailbox;
                    var sequence = requestOptions.Slice(0, index.Value);
                    var numMessages = mailbox.NumberOfMessages;
                    var sequenceIds = 
                        SequenceSetParser.ParseBySequenceId(LexiConstants.AsString(sequence), numMessages);
                    if (sequenceIds.Count > 0)
                    {
                        foreach (var sequenceId in sequenceIds)
                        {
                            var message = mailbox.GetMessage(sequenceId);
                            if (message != null)
                            {
                                PrintMessageFlags(response, message);
                                response.AppendLine(requestId, ImapResponse.Ok, completedHint);
                            }
                        }
                    }
                    else
                    {
                        response.AppendLine(requestId, ImapResponse.No, noneHint);
                    }
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

        private void PrintMessageFlags(ImapResponse response, ImapMessage message) { }
    }
}
