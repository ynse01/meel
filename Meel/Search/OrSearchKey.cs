using System;

namespace Meel.Search
{
    public class OrSearchKey : ISearchKey
    {
        private ISearchKey left;
        private ISearchKey right;

        public OrSearchKey(ISearchKey left, ISearchKey right)
        {
            this.left = left;
            this.right = right;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return left.Matches(message, sequence) || right.Matches(message, sequence);
        }
    }
}
