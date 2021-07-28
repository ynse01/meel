
using Meel.Responses;
using System.Buffers;

namespace Meel.Commands
{
    public interface IImapCommand
    {
        int Execute(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> requestOptions, ref ImapResponse response);

        void ReceiveLiteral(ConnectionContext context, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> literal, ref ImapResponse response);
    }
}
