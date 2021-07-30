using System;
using Meel.Parsing;

namespace Meel.Search
{
    public class OnSearchKey : ISearchKey
    {
        private DateTimeOffset date;

        public OnSearchKey(ReadOnlySpan<byte> before)
        {
            date = DateTimeOffset.Parse(before.AsString()).Date;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return message.Message.Date.Date.CompareTo(date) == 0;
        }
    }
}
