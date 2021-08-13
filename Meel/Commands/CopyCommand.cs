using Meel.Parsing;
using Meel.Responses;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class CopyCommand : ImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("COPY completed");
        private static readonly byte[] tryCreateHint = Encoding.ASCII.GetBytes("[TRYCREATE]");
        private static readonly byte[] failedHint = Encoding.ASCII.GetBytes("COPY failed");
        private static readonly byte[] noMessagesHint =
            Encoding.ASCII.GetBytes("Cannot find those messages");
        private static readonly byte[] cannotHint =
            Encoding.ASCII.GetBytes("Cannot find mailbox with that name");
        private static readonly byte[] missingHint =
            Encoding.ASCII.GetBytes("Need to specify a sequence and a destination mailbox");
        private static readonly byte[] modeHint =
            Encoding.ASCII.GetBytes("Need to be in Selected mode for this command");

        public CopyCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Selected && context.SelectedMailbox != null) {
                var index = requestOptions.IndexOf(LexiConstants.Space);
                if (index > 0) {
                    var name = requestOptions.Slice(index + 1).AsString();
                    var source = context.SelectedMailbox;
                    var maxMessage = (uint)source.NumberOfMessages;
                    var sequence = SequenceSetParser.Parse(requestOptions.Slice(0, index), maxMessage);
                    if (sequence.Count > 0)
                    {
                        var destination = station.SelectMailbox(context.Username, name);
                        if (destination != null)
                        {
                            var result = station.CopyMessages(sequence, source, destination);
                            if (result)
                            {
                                response.Allocate(6 + requestId.Length + completedHint.Length);
                                response.AppendLine(requestId, ImapResponse.Ok, completedHint);
                            } else
                            {
                                response.Allocate(6 + requestId.Length + failedHint.Length);
                                response.AppendLine(requestId, ImapResponse.No, failedHint);
                            }
                        }
                        else
                        {
                            response.Allocate(7 + requestId.Length + tryCreateHint.Length + cannotHint.Length);
                            response.AppendLine(requestId, ImapResponse.No, tryCreateHint, cannotHint);
                        }
                    } else
                    {
                        response.Allocate(6 + requestId.Length + noMessagesHint.Length);
                        response.AppendLine(requestId, ImapResponse.No, noMessagesHint);
                    }
                } else
                {
                    response.Allocate(7 + requestId.Length + missingHint.Length);
                    response.AppendLine(requestId, ImapResponse.Bad, missingHint);
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
