using Meel.Commands;
using Meel.Responses;
using System;
using System.Buffers;
using System.Collections.Generic;

namespace Meel
{
    public class LocalRequestResponsePlane : IRequestResponsePlane
    {
        private Dictionary<long, ConnectionContext> contexts = new Dictionary<long, ConnectionContext>();
        private static readonly object syncRoot = new object();

        private CommandFactory factory;

        public LocalRequestResponsePlane(IMailStation station)
        {
            factory = new CommandFactory(station);
        }

        public IIdentifyable CreateSession(long uid)
        {
            return new ConnectionContext(uid);
        }

        public int HandleRequest(
            ImapCommands request,
            IIdentifyable session, 
            ReadOnlySequence<byte> requestId,
            ReadOnlySpan<byte> options,
            ref ImapResponse response
        ) {
            var command = factory.GetCommand(request);
            var context = (ConnectionContext)session;
            return command.Execute(context, requestId, options, ref response);
        }

        public void ReceiveLiteral(
            ImapCommands request,
            IIdentifyable session,
            ReadOnlySequence<byte> requestId,
            ReadOnlySequence<byte> literal,
            ref ImapResponse response
        ) {
            var command = factory.GetCommand(request);
            var context = (ConnectionContext)session;
            command.ReceiveLiteral(context, requestId, literal, ref response);
        }
    }
}
