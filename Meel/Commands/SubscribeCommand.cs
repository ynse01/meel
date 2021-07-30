using Meel.Parsing;
using Meel.Responses;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class SubscribeCommand : ImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("SUBSCRIBE completed");
        private static readonly byte[] cannotHint =
            Encoding.ASCII.GetBytes("Cannot subscribe to mailbox with that name");
        private static readonly byte[] missingHint = 
            Encoding.ASCII.GetBytes("Need to specify the mailbox name to subscribe to");
        private static readonly byte[] authHint = 
            Encoding.ASCII.GetBytes("Need to be Authenticated for this command");

        public SubscribeCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected) {
                if (!requestOptions.IsEmpty)
                {
                    var name = requestOptions.AsString();
                    var isSubscribed = station.SetSubscription(context.Username, name, true);
                    if (isSubscribed)
                    {
                        response.Allocate(0);
                        response.AppendLine(requestId, ImapResponse.Ok, completedHint);
                    } else
                    {
                        response.Allocate(6 + requestId.Length + cannotHint.Length);
                        response.AppendLine(requestId, ImapResponse.No, cannotHint);
                    }
                } else
                {
                    response.Allocate(6 + requestId.Length + missingHint.Length);
                    response.AppendLine(requestId, ImapResponse.No, missingHint);
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
