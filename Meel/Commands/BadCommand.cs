﻿using Meel.Responses;
using System.Buffers;
using System.Text;

namespace Meel.Commands
{
    public class BadCommand : IImapCommand
    {
        private static readonly byte[] invalidHint = 
            Encoding.ASCII.GetBytes("Invalid command syntax or unknown command");

        public int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> requestOptions, ref ImapResponse response)
        {
            response.Allocate(7 + requestId.Length + invalidHint.Length);
            response.AppendLine(requestId, ImapResponse.Bad, invalidHint);
            return 0;
        }

        public void ReceiveLiteral(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> literal, ref ImapResponse response)
        {
            // Not applicable
        }
    }
}
