using System;

namespace Meel.Search
{
    public class SmallerSearchKey : ISearchKey
    {
        private uint size;
        
        public SmallerSearchKey(uint size)
        {
            this.size = size;
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Size;
        }

        public bool Matches(ImapMessage message, uint sequenceId)
        {
            return message.Size < size;
        }
    }
}
