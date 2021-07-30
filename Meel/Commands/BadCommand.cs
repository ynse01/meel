using Meel.Responses;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class BadCommand : ImapCommand
    {
        private static readonly byte[] invalidHint = 
            Encoding.ASCII.GetBytes("Invalid command syntax or unknown command");

        public BadCommand(IMailStation station) : base(station) { }

        public override int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> requestOptions, ref ImapResponse response)
        {
            response.Allocate(7 + requestId.Length + invalidHint.Length);
            response.AppendLine(requestId, ImapResponse.Bad, invalidHint);
            return 0;
        }
    }
}
