using System;

namespace Meel.Search
{
    public class DeletedSearchKey : ISearchKey
    {
        private bool inverted;

        public DeletedSearchKey(bool inverted)
        {
            this.inverted = inverted;
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Flags;
        }

        public bool Matches(ImapMessage message, uint sequenceId)
        {
            return inverted ^ message.Deleted;
        }
    }
}
