using Meel.Parsing;
using Meel.Responses;
using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace Meel.Commands
{
    public class FetchCommand : IImapCommand
    {
        private IMailStation station;

        public FetchCommand(IMailStation station)
        {
            this.station = station;
        }

        public ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions)
        {
            var response = new ImapResponse();
            if (context.State == SessionState.Selected) {
                var parts = requestOptions.Split(' ', 2);
                if (parts.Length == 2)
                {
                    var mailbox = context.SelectedMailbox;
                    var sequenceIds = SequenceSetParser.ParseBySequenceId(requestOptions, mailbox.NumberOfMessages);
                    if (sequenceIds.Any())
                    {
                        foreach (var sequenceId in sequenceIds)
                        {
                            var message = mailbox.GetMessage(sequenceId);
                            if (message != null)
                            {
                                PrintMessagePart(response, message, parts[1]);
                                response.WriteLine(requestId, "OK", "FETCH completed");
                            }
                        }
                    }
                    else
                    {
                        response.WriteLine(requestId, "NO", "No messages found");
                    }
                } else
                {
                    response.WriteLine(requestId, "BAD", "Need to specify a sequence number and item name");
                }
            } else
            {
                response.WriteLine(requestId, "BAD", "Need to be in SELECTED mode for this command");
            }
            return response;
        }

        public ImapResponse ReceiveLiteral(ConnectionContext context, string requestId, List<string> literal)
        {
            // Not applicable
            return null;
        }


        private void PrintMessagePart(ImapResponse response, ImapMessage message, string parts)
        {
            // TODO: Filter on part. For now return entire message.
            response.Write(message.Message.ToString());
        }
    }
}
