using System;

namespace Meel.Search
{
    public class OnSearchKey : ISearchKey
    {
        private DateTimeOffset date;

        public OnSearchKey(DateTimeOffset before)
        {
            date = before.Date;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return message.Message.Date.Date.CompareTo(date) == 0;
        }
    }
}
