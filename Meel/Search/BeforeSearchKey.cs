using System;
using Meel.Parsing;

namespace Meel.Search
{
    public class BeforeSearchKey : ISearchKey
    {
        private DateTimeOffset date;

        public BeforeSearchKey(ReadOnlySpan<byte> before)
        {
            date = DateTimeOffset.Parse(before.AsString()).Date;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return message.Message.Date.Date.CompareTo(date) < 0;
        }
    }
}
