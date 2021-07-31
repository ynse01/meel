using System;

namespace Meel.Search
{
    public class LargerSearchKey : ISearchKey
    {
        private uint size;
        
        public LargerSearchKey(uint size)
        {
            this.size = size;
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Size;
        }

        public bool Matches(ImapMessage message, uint sequenceId)
        {
            return message.Size > size;
        }
    }
}
