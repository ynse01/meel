using System;

namespace Meel.Search
{
    public class NotSearchKey : ISearchKey
    {
        private ISearchKey inner;

        public NotSearchKey(ISearchKey inner)
        {
            this.inner = inner;
        }

        public SearchDepth GetSearchDepth()
        {
            return inner.GetSearchDepth();
        }

        public bool Matches(ImapMessage message, int sequenceId)
        {
            return !inner.Matches(message, sequenceId);
        }
    }
}
