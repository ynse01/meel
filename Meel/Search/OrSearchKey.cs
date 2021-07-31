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

        public SearchDepth GetSearchDepth()
        {
            return left.GetSearchDepth() | right.GetSearchDepth();
        }

        public bool Matches(ImapMessage message, int sequenceId)
        {
            return left.Matches(message, sequenceId) || right.Matches(message, sequenceId);
        }
    }
}
