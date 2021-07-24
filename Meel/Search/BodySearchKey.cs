using MimeKit.Text;
using System;

namespace Meel.Search
{
    public class BodySearchKey : ISearchKey
    {
        private string needle;

        public BodySearchKey(string needle)
        {
            this.needle = needle;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return message.Message.GetTextBody(TextFormat.Text).Contains(needle, StringComparison.OrdinalIgnoreCase);
        }
    }
}
