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

        public bool Matches(ImapMessage message, int sequence)
        {
            return inverted ^ message.Deleted;
        }
    }
}
