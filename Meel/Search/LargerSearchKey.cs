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

        public bool Matches(ImapMessage message, int sequence)
        {
            return message.Size > size;
        }
    }
}
