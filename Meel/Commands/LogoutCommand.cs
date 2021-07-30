using Meel.Responses;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class LogoutCommand : ImapCommand
    {
        private static readonly byte[] completedHint =
            Encoding.ASCII.GetBytes("LOGOUT completed");
        private static readonly byte[] terminationHint = 
            Encoding.ASCII.GetBytes("MEEL server terminating connection");

        public LogoutCommand(IMailStation station) : base(station) { }
        
        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> requestOptions, ref ImapResponse response)
        {
            response.Allocate(14 + terminationHint.Length + requestId.Length + completedHint.Length);
            response.AppendLine(ImapResponse.Untagged, ImapResponse.Bye, terminationHint);
            response.AppendLine(requestId, ImapResponse.Ok, completedHint);
            return 0;
        }
    }
}
