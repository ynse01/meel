using System;

namespace Meel.Search
{
    public class DraftSearchKey : ISearchKey
    {
        private bool inverted;

        public DraftSearchKey(bool inverted)
        {
            this.inverted = inverted;
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Flags;
        }
        
        public bool Matches(ImapMessage message, int sequenceId)
        {
            return inverted ^ message.Draft;
        }
    }
}
