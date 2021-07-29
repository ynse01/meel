using Meel.Parsing;
using Meel.Responses;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class LoginCommand : ImapCommand
    {
        private static readonly byte[] acceptHint =
            Encoding.ASCII.GetBytes("LOGIN accepted");
        private static readonly byte[] argsHint =
            Encoding.ASCII.GetBytes("Need to specify username and password");
        private static readonly byte[] invalidHint =
            Encoding.ASCII.GetBytes("LOGIN invalid username or password provided");

        public LoginCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> requestOptions, ref ImapResponse response)
        {
            if (!requestOptions.IsEmpty)
            {
                var index = requestOptions.PositionOf(LexiConstants.Space);
                if (index.HasValue)
                {
                    response.AppendLine(requestId, ImapResponse.Ok, acceptHint);
                    context.Username = requestOptions.Slice(0, index.Value).AsString();
                    context.State = SessionState.Authenticated;
                } else
                {
                    response.AppendLine(requestId, ImapResponse.No, invalidHint);
                    context.State = SessionState.NotAuthenticated;
                }
            } else
            {
                response.Allocate(7 + requestId.Length + argsHint.Length);
                response.AppendLine(requestId, ImapResponse.Bad, argsHint);
            }
            return 0;
        }
    }
}
