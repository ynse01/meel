using System;

namespace Meel.Search
{
    public class SinceSearchKey : ISearchKey
    {
        private DateTimeOffset date;

        public SinceSearchKey(DateTimeOffset before)
        {
            date = before.Date;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return message.Message.Date.Date.CompareTo(date) > 0;
        }
    }
}
