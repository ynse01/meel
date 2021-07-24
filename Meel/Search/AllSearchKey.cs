using System;

namespace Meel.Search
{
    public class AllSearchKey : ISearchKey
    {
        public bool Matches(ImapMessage message, int sequence)
        {
            return true;
        }
    }
}
