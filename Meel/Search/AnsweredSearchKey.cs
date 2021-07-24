using System;

namespace Meel.Search
{
    public class AnsweredSearchKey : ISearchKey
    {
        private bool inverted;

        public AnsweredSearchKey(bool inverted)
        {
            this.inverted = inverted;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return !(inverted ^ message.Answered);
        }
    }
}
