using Meel.DataItems;
using Meel.Parsing;
using Meel.Responses;
using MimeKit;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Meel.Commands
{
    public class FetchCommand : ImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("FETCH completed");
        private static readonly byte[] noneHint = Encoding.ASCII.GetBytes("No messages found");
        private static readonly byte[] argsHint = 
            Encoding.ASCII.GetBytes("Need to specify a sequence number and item name");
        private static readonly byte[] modeHint =
            Encoding.ASCII.GetBytes("Need to be in SELECTED mode for this command");
        
        public FetchCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Selected) {
                var index = requestOptions.IndexOf(LexiConstants.Space);
                if (index >= 0)
                {
                    var mailbox = context.SelectedMailbox;
                    var sequence = requestOptions.Slice(0, index);
                    var sequenceIds = SequenceSetParser.Parse(sequence, (uint)mailbox.NumberOfMessages);
                    var fetchQuery = requestOptions.Slice(index + 1);
                    var fetchItem = DataItemsParser.Parse(fetchQuery);
                    if (sequenceIds.Count > 0)
                    {
                        using (var stream = response.GetStream())
                        {
                            foreach (var item in sequenceIds)
                            {
                                var message = mailbox.GetMessage(item);
                                if (message != null)
                                {
                                    PrintMessagePart(stream, message, fetchItem);
                                }
                            }
                        }
                        response.Allocate(6 + requestId.Length + noneHint.Length);
                        response.AppendLine(requestId, ImapResponse.Ok, completedHint);
                    }
                    else
                    {
                        response.Allocate(6 + requestId.Length + noneHint.Length);
                        response.AppendLine(requestId, ImapResponse.No, noneHint);
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

        private void PrintMessagePart(Stream stream, ImapMessage message, DataItem dataItem)
        {
            // TODO: Filter on part. For now return entire message.
            message.Message.WriteTo(stream);
        }
    }
}
