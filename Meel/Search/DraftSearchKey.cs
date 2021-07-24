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

        public bool Matches(ImapMessage message, int sequence)
        {
            return !(inverted ^ message.Draft);
        }
    }
}
