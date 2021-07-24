
using Meel.Responses;
using System.Collections.Generic;

namespace Meel.Commands
{
    public interface IImapCommand
    {
        ImapResponse Execute(ConnectionContext context, string requestId, string requestOptions);

        ImapResponse ReceiveLiteral(ConnectionContext context, string requestId, List<string> literal);
    }
}
