using System;

namespace Meel.Search
{
    public class OldSearchKey : ISearchKey
    {
        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Flags;
        }
        
        public bool Matches(ImapMessage message, uint sequenceId)
        {
            return !message.Recent;
        }
    }
}
