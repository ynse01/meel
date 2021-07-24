using System;

namespace Meel.Search
{
    public class NewSearchKey : ISearchKey
    {
        private bool inverted;

        public NewSearchKey(bool inverted)
        {
            this.inverted = inverted;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return !(inverted ^ message.Recent);
        }
    }
}
