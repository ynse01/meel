using Meel.Commands;
using Meel.Responses;
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

        public ImapResponse HandleRequest(ImapCommands request, long sessionId, string requestId, string options)
        {
            var command = factory.GetCommand(request);
            var context = GetContext(sessionId);
            return command.Execute(context, requestId, options);
        }

        public ImapResponse ReceiveLiteral(ImapCommands request, long sessionId, string requestId, List<string> literal)
        {
            var command = factory.GetCommand(request);
            var context = GetContext(sessionId);
            return command.ReceiveLiteral(context, requestId, literal);
        }

        private ConnectionContext GetContext(long sessionId)
        {
            ConnectionContext context;
            lock (syncRoot)
            {
                if (!contexts.TryGetValue(sessionId, out context))
                {
                    context = new ConnectionContext(sessionId);
                    contexts.Add(sessionId, context);
                }
            }
            return context;
        }
    }
}
