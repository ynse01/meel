using System;

namespace Meel.Search
{
    public class LargerSearchKey : ISearchKey
    {
        private int size;
        
        public LargerSearchKey(int size)
        {
            this.size = size;
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Size;
        }

        public bool Matches(ImapMessage message, int sequenceId)
        {
            return message.Size > size;
        }
    }
}
