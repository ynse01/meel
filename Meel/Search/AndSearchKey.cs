using System;

namespace Meel.Search
{
    public class AndSearchKey : ISearchKey
    {
        private ISearchKey left;
        private ISearchKey right;

        public AndSearchKey(ISearchKey left, ISearchKey right)
        {
            this.left = left;
            this.right = right;
        }

        public SearchDepth GetSearchDepth()
        {
            return left.GetSearchDepth() | right.GetSearchDepth();
        }

        public bool Matches(ImapMessage message, int sequenceId)
        {
            return left.Matches(message, sequenceId) && right.Matches(message, sequenceId);
        }
    }
}
