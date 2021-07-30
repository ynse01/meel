using System;

namespace Meel.Search
{
    public class OldSearchKey : ISearchKey
    {
        public bool Matches(ImapMessage message, int sequence)
        {
            return !message.Recent;
        }
    }
}
