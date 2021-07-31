using MimeKit.Text;
using System;
using Meel.Parsing;

namespace Meel.Search
{
    public class BodySearchKey : ISearchKey
    {
        private string needle;

        public BodySearchKey(ReadOnlySpan<byte> needle)
        {
            this.needle = needle.AsString();
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Header;
        }

        public bool Matches(ImapMessage message, uint sequenceId)
        {
            return message.Message.GetTextBody(TextFormat.Text).Contains(needle, StringComparison.OrdinalIgnoreCase);
        }
    }
}
