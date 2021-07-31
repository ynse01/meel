using System;

namespace Meel.Search
{
    public class SeenSearchKey : ISearchKey
    {
        private bool inverted;

        public SeenSearchKey(bool inverted)
        {
            this.inverted = inverted;
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Flags;
        }

        public bool Matches(ImapMessage message, int sequenceId)
        {
            return inverted ^ message.Seen;
        }
    }
}
