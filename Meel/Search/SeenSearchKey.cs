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

        public bool Matches(ImapMessage message, int sequence)
        {
            return inverted ^ message.Seen;
        }
    }
}
