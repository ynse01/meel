using System;

namespace Meel.Search
{
    public class SentBeforeSearchKey : ISearchKey
    {
        private DateTimeOffset date;

        public SentBeforeSearchKey(DateTimeOffset before)
        {
            date = before.Date;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return message.Message.Date.Date.CompareTo(date) < 0;
        }
    }
}
