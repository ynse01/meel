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

        public bool Matches(ImapMessage message, int sequence)
        {
            return !(inverted ^ message.Recent);
        }
    }
}
