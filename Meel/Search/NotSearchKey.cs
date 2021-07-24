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

        public bool Matches(ImapMessage message, int sequence)
        {
            return !inner.Matches(message, sequence);
        }
    }
}
