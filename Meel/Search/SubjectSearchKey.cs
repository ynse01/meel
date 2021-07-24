using System;

namespace Meel.Search
{
    public class SubjectSearchKey : ISearchKey
    {
        private string needle;

        public SubjectSearchKey(string needle)
        {
            this.needle = needle;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            var subject = message.Message.Subject;
            return subject.Contains(needle, StringComparison.OrdinalIgnoreCase);
        }
    }
}
