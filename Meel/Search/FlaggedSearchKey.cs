using System;

namespace Meel.Search
{
    public class FlaggedSearchKey : ISearchKey
    {
        private bool inverted;

        public FlaggedSearchKey(bool inverted)
        {
            this.inverted = inverted;
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Flags;
        }
        
        public bool Matches(ImapMessage message, uint sequenceId)
        {
            return inverted ^ message.Flagged;
        }
    }
}
