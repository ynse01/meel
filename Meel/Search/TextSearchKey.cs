using MimeKit.Text;
using System;
using Meel.Parsing;

namespace Meel.Search
{
    public class TextSearchKey : ISearchKey
    {
        private string needle;

        public TextSearchKey(ReadOnlySpan<byte> needle)
        {
            this.needle = needle.AsString();
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return message.Message.GetTextBody(TextFormat.Text).Contains(needle, StringComparison.OrdinalIgnoreCase);
        }
    }
}
