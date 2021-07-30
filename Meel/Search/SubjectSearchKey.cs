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

        public bool Matches(ImapMessage message, int sequence)
        {
            var subject = message.Message.Subject;
            return subject.Contains(needle, StringComparison.OrdinalIgnoreCase);
        }
    }
}
