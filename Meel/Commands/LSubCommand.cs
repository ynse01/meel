using Meel.Parsing;
using Meel.Responses;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class LSubCommand : ImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("LSUB completed");
        private static readonly byte[] cannotHint = Encoding.ASCII.GetBytes("Can't list that reference or name");
        private static readonly byte[] argsHint =
            Encoding.ASCII.GetBytes("Need to specify a mailbox name and a reference");
        private static readonly byte[] authHint =
            Encoding.ASCII.GetBytes("Need to be Authenticated for this command");

        public LSubCommand(IMailStation station) : base(station) { }

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
                            ListCommand.PrepareListResponse(ref response, completeList, padding, true);
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
    }
}
