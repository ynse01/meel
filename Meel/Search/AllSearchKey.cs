using System;

namespace Meel.Search
{
    public class AllSearchKey : ISearchKey
    {
        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.None;
        }
        
        public bool Matches(ImapMessage message, uint sequenceId)
        {
            return true;
        }
    }
}
