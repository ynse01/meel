using Meel.Parsing;
using Meel.Responses;
using MimeKit;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class AppendCommand : ImapCommand
    {
        private static MetadataKey MailboxKey;
        private static readonly byte[] createHint = 
            Encoding.ASCII.GetBytes("[TRYCREATE] No mailbox found by that name");
        private static readonly byte[] readyHint = Encoding.ASCII.GetBytes("Ready for literal data");
        private static readonly byte[] missingNameHint = Encoding.ASCII.GetBytes("No mailbox name specified");
        private static readonly byte[] needAuthenticateHint =
            Encoding.ASCII.GetBytes("Need to be Authenticated for this command");
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("APPEND completed");
        private static readonly byte[] errorHint = Encoding.ASCII.GetBytes("APPEND Internal error");

        public AppendCommand(IMailStation station) : base(station) { }

        public override void Initialize()
        {
            MailboxKey = MetadataKey.Create();
        }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> requestOptions, ref ImapResponse response)
        {
            int result = -1;
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected)
            {
                if (!requestOptions.IsEmpty)
                {
                    var index = requestOptions.PositionOf(LexiConstants.Space);
                    if (index.HasValue)
                    {
                        var name = requestOptions.AsString();
                        var sizeSpan = FindBetweenCurlyBraces(requestOptions);
                        var size = ParseNumber(sizeSpan);
                        // TODO: Handle optional flags and date/time
                        var mailbox = station.SelectMailbox(context.Username, name);
                        if (mailbox != null)
                        {
                            context.ExpectLiteral = true;
                            context.SetMetadata(MailboxKey, mailbox);
                            response.Allocate(3 + requestId.Length + readyHint.Length);
                            response.AppendLine(requestId, readyHint);
                            result = size;
                        }
                        else
                        {
                            response.Allocate(6 + requestId.Length + createHint.Length);
                            response.AppendLine(requestId, ImapResponse.No, createHint);
                        }
                    }
                    else
                    {
                        response.Allocate(6 + requestId.Length + missingNameHint.Length);
                        response.AppendLine(requestId, ImapResponse.No, missingNameHint);
                    }
                }
                else
                {
                    response.Allocate(6 + requestId.Length + missingNameHint.Length);
                    response.AppendLine(requestId, ImapResponse.No, missingNameHint);
                }
            }
            else
            {
                response.Allocate(6 + requestId.Length + needAuthenticateHint.Length);
                response.AppendLine(requestId, ImapResponse.No, needAuthenticateHint);
            }
            return result;
        }

        public override void ReceiveLiteral(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> literal, ref ImapResponse response)
        {
            if (context.TryGetMetadata(MailboxKey, out Mailbox mailbox))
            {
                var message = ParseMessageLiteral(literal);
                station.AppendToMailbox(mailbox, message);
                response.Allocate(6 + requestId.Length + completedHint.Length);
                response.AppendLine(requestId, ImapResponse.Ok, completedHint);
            } else
            {
                response.Allocate(6 + requestId.Length + errorHint.Length);
                response.AppendLine(requestId, ImapResponse.Bad, errorHint);
            }
            context.RemoveMetadata(MailboxKey);
            context.ExpectLiteral = false;
        }

        private static ReadOnlySequence<byte> FindBetweenCurlyBraces(ReadOnlySequence<byte> haystack)
        {
            return haystack;
        }

        private static int ParseNumber(ReadOnlySequence<byte> span)
        {
            return 0;
        }

        private ImapMessage ParseMessageLiteral(ReadOnlySequence<byte> literal)
        {
            return null;
        }
    }
}
