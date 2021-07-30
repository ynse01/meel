
using Meel.Responses;
using System;
using System.Buffers;

namespace Meel.Commands
{
    public abstract class ImapCommand
    {
        protected readonly IMailStation station;

        public ImapCommand(IMailStation station)
        {
            this.station = station;
        }

        public virtual void Initialize() { }

        public abstract int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> requestOptions, ref ImapResponse response);

        public virtual void ReceiveLiteral(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> literal, ref ImapResponse response)
        {
            // Not implemented yet
        }
    }
}
