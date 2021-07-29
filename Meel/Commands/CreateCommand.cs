using Meel.Parsing;
using Meel.Responses;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class CreateCommand : ImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("CREATE completed");
        private static readonly byte[] cannotHint =
            Encoding.ASCII.GetBytes("Cannot create mailbox with that name");
        private static readonly byte[] missingHint =
            Encoding.ASCII.GetBytes("Need to specify the mailbox name to create");
        private static readonly byte[] authHint =
            Encoding.ASCII.GetBytes("Need to be Authenticated for this command");

        public CreateCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected) {
                if (!requestOptions.IsEmpty)
                {
                    var name = requestOptions.AsString();
                    var isCreated = station.CreateMailbox(context.Username, name);
                    if (isCreated)
                    {
                        response.Allocate(6 + requestId.Length + completedHint.Length);
                        response.AppendLine(requestId, ImapResponse.Ok, completedHint);
                    } else
                    {
                        response.Allocate(6 + requestId.Length + cannotHint.Length);
                        response.AppendLine(requestId, ImapResponse.No, cannotHint);
                    }
                } else
                {
                    response.Allocate(7 + requestId.Length + missingHint.Length);
                    response.AppendLine(requestId, ImapResponse.Bad, missingHint);
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
