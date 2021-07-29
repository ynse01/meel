using Meel.Parsing;
using Meel.Responses;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class RenameCommand : ImapCommand
    {
        private static readonly byte[] completedHint = Encoding.ASCII.GetBytes("RENAME completed");
        private static readonly byte[] cannotHint =
            Encoding.ASCII.GetBytes("Cannot rename mailbox with that name");
        private static readonly byte[] argsToHint =
            Encoding.ASCII.GetBytes("Need to specify the name to rename to");
        private static readonly byte[] argsHint =
            Encoding.ASCII.GetBytes("Need to specify the names to rename to and from");
        private static readonly byte[] authHint = 
            Encoding.ASCII.GetBytes("Need to be Authenticated for this command");
        
        public RenameCommand(IMailStation station) : base(station) { }
        
        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> requestOptions, ref ImapResponse response)
        {
            if (context.State == SessionState.Authenticated || context.State == SessionState.Selected) {
                if (!requestOptions.IsEmpty)
                {
                    var index = requestOptions.PositionOf(LexiConstants.Space);
                    if (index.HasValue)
                    {
                        var oldName = requestOptions.Slice(0, index.Value).AsString();
                        var newName = requestOptions.Slice(index.Value).AsString();
                        var isRenamed = station.RenameMailbox(context.Username, oldName, newName);
                        if (isRenamed)
                        {
                            response.Allocate(6 + requestId.Length + completedHint.Length);
                            response.AppendLine(requestId, ImapResponse.Ok, completedHint);
                        }
                        else
                        {
                            response.Allocate(6 + requestId.Length + cannotHint.Length);
                            response.AppendLine(requestId, ImapResponse.No, cannotHint);
                        }
                    } else
                    {
                        response.Allocate(7 + requestId.Length + argsToHint.Length);
                        response.AppendLine(requestId, ImapResponse.Bad, argsToHint);
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
