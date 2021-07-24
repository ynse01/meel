using System;

namespace Meel.Search
{
    public class SentSinceSearchKey : ISearchKey
    {
        private DateTimeOffset date;

        public SentSinceSearchKey(DateTimeOffset before)
        {
            date = before.Date;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return message.Message.Date.Date.CompareTo(date) > 0;
        }
    }
}
