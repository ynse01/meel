using Meel.Responses;
using MimeKit;
using System.Collections.Generic;

namespace Meel.Commands
{
    public class AppendCommand : IImapCommand
    {
        private const string MailboxKey = "AppendToMailbox";
        private IMailStation station;

        public AppendCommand(IMailStation station)
        {
            this.station = station;
        }

        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected)
            {
                if (!string.IsNullOrEmpty(requestOptions))
                {
                    var parts = requestOptions.Split(' ');
                    if (parts.Length > 0)
                    {
                        var name = parts[0];
                        // TODO: Handle optional flags and date/time
                        var mailbox = station.SelectMailbox(context.Username, name);
                        if (mailbox != null)
                        {
                            context.ExpectLiteral = true;
                            context.SetMetadata(MailboxKey, mailbox);
                            response.WriteLine("+", "Ready for literal data");
                            // TODO: Handle expected literal size
                            response.ExpectLiteralOfSize = 1;
                        }
                        else
                        {
                            response.WriteLine(requestId, "NO", "[TRYCREATE]", "No mailbox found by that name");
                        }
                    }
                    else
                    {
                        response.WriteLine(requestId, "NO", "No mailbox name specified");
                    }
                }
                else
                {
                    response.WriteLine(requestId, "BAD", "Need to specify mailbox name");
                }
            } else
            {
                response.WriteLine(requestId, "BAD", "Need to be Authenticated for this command");
            }
            return response;
        }

        public ImapResponse ReceiveLiteral(ConnectionContext context, string requestId, List<string> literal)
        {
            var response = new ImapResponse();
            if (context.TryGetMetadata(MailboxKey, out Mailbox mailbox))
            {
                var message = ParseMessageLiteral(literal);
                station.AppendToMailbox(mailbox, message);
                response.WriteLine(requestId, "OK", "APPEND completed");
            } else
            {
                response.WriteLine(requestId, "BAD", "Internal error");
            }
            context.RemoveMetadata(MailboxKey);
            context.ExpectLiteral = false;
            return response;
        }

        private ImapMessage ParseMessageLiteral(List<string> literal)
        {
            return null;
        }
    }
}
