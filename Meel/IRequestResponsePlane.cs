using Meel.Commands;
using Meel.Responses;
using System;
using System.Buffers;

namespace Meel
{
    public interface IIdentifyable
    {
        long Uid { get; }
    }

    public interface IRequestResponsePlane
    {
        IIdentifyable CreateSession(long uid);

        int HandleRequest(ImapCommands command, IIdentifyable sessionId, ReadOnlySequence<byte> requestId, ReadOnlySpan<byte> options, ref ImapResponse response);

        void ReceiveLiteral(ImapCommands command, IIdentifyable sessionId, ReadOnlySequence<byte> requestId, ReadOnlySequence<byte> literal, ref ImapResponse response);
    }
}
