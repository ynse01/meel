using System;

namespace Meel.Search
{
    public class RecentSearchKey : ISearchKey
    {
        private bool inverted;

        public RecentSearchKey(bool inverted)
        {
            this.inverted = inverted;
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Flags;
        }

        public bool Matches(ImapMessage message, uint sequenceId)
        {
            return inverted ^ message.Recent;
        }
    }
}
