using System;

namespace Meel.Search
{
    public class HeaderSearchKey : ISearchKey
    {
        private string name;
        private string value;

        public HeaderSearchKey(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return message.Message.Headers[name].Contains(value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
