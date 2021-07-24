using Meel.Commands;
using Meel.Responses;
using System.Collections.Generic;

namespace Meel
{
    public interface IRequestResponsePlane
    {
        ImapResponse HandleRequest(ImapCommands command, long sessionId, string requestId, string options);

        ImapResponse ReceiveLiteral(ImapCommands command, long sessionId, string requestId, List<string> literal);
    }
}
