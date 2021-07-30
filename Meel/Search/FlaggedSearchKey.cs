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

        public bool Matches(ImapMessage message, int sequence)
        {
            return inverted ^ message.Flagged;
        }
    }
}
