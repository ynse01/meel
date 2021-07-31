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

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Header;
        }

        public bool Matches(ImapMessage message, int sequenceId)
        {
            return message.Message.Date.Date.CompareTo(date) == 0;
        }
    }
}
