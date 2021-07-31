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

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Flags;
        }

        public bool Matches(ImapMessage message, uint sequenceId)
        {
            return inverted ^ message.Answered;
        }
    }
}
