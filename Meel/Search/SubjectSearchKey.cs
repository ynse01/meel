using System;
using Meel.Parsing;

namespace Meel.Search
{
    public class SubjectSearchKey : ISearchKey
    {
        private string needle;

        public SubjectSearchKey(ReadOnlySpan<byte> needle)
        {
            this.needle = needle.AsString();
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Header;
        }

        public bool Matches(ImapMessage message, uint sequenceId)
        {
            var subject = message.Message.Subject;
            return subject.Contains(needle, StringComparison.OrdinalIgnoreCase);
        }
    }
}
