using Meel.Parsing;
using Meel.Responses;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class ExamineCommand : ImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("EXAMINE completed");
        private static readonly byte[] readOnlyHint = Encoding.ASCII.GetBytes("[READ-ONLY]");
        private static readonly byte[] missingHint =
            Encoding.ASCII.GetBytes("No mailbox by that name");
        private static readonly byte[] argsHint =
            Encoding.ASCII.GetBytes("Need to specify a mailbox name");
        private static readonly byte[] authHint =
            Encoding.ASCII.GetBytes("Need to be Authenticated for this command");
        
        public ExamineCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected)
            {
                if (!requestOptions.IsEmpty) { 
                    var name = requestOptions.AsString();
                    var mailbox = station.SelectMailbox(context.Username, name);
                    if (mailbox != null)
                    {
                        context.SetSelectedMailbox(mailbox);
                        var padding = 7 + requestId.Length + readOnlyHint.Length + completedHint.Length;
                        SelectCommand.PrepareResponse(ref response, mailbox, padding, false);
                        response.AppendLine(requestId, ImapResponse.Ok, readOnlyHint, completedHint);
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
    }
}
