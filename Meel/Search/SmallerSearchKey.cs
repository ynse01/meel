using System;

namespace Meel.Search
{
    public class SmallerSearchKey : ISearchKey
    {
        private int size;
        
        public SmallerSearchKey(int size)
        {
            this.size = size;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return message.Size < size;
        }
    }
}
