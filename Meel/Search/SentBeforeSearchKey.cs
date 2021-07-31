using System;
using Meel.Parsing;

namespace Meel.Search
{
    public class SentBeforeSearchKey : ISearchKey
    {
        private DateTimeOffset date;

        public SentBeforeSearchKey(ReadOnlySpan<byte> before)
        {
            date = DateTimeOffset.Parse(before.AsString()).Date;
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Header;
        }
        
        public bool Matches(ImapMessage message, uint sequenceId)
        {
            return message.Message.Date.Date.CompareTo(date) < 0;
        }
    }
}
