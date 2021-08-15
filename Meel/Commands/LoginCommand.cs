using Meel.Parsing;
using Meel.Responses;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class LoginCommand : ImapCommand
    {
        private static readonly byte[] completedHint =
            Encoding.ASCII.GetBytes("LOGIN completed");
        private static readonly byte[] argsHint =
            Encoding.ASCII.GetBytes("Need to specify username and password");
        private static readonly byte[] invalidHint =
            Encoding.ASCII.GetBytes("LOGIN invalid username or password provided");

        public LoginCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> requestOptions, ref ImapResponse response)
        {
            if (!requestOptions.IsEmpty)
            {
                var index = requestOptions.IndexOf(LexiConstants.Space);
                if (index >= 0)
                {
                    var username = requestOptions.Slice(0, index).AsString();
                    var password = requestOptions.Slice(index + 1).AsString();
                    // TODO: Implement checking credentials
                    response.Allocate(7 + requestId.Length + completedHint.Length);
                    response.AppendLine(requestId, ImapResponse.Ok, completedHint);
                    context.Username = username;
                    context.State = SessionState.Authenticated;
                } else
                {
                    response.Allocate(6 + requestId.Length + invalidHint.Length);
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
