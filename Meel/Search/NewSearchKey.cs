using System;

namespace Meel.Search
{
    public class NewSearchKey : ISearchKey
    {
        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Flags;
        }

        public bool Matches(ImapMessage message, uint sequenceId)
        {
            return !message.Seen && message.Recent;
        }
    }
}
