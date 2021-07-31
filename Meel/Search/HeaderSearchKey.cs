using System;
using Meel.Parsing;

namespace Meel.Search
{
    public class HeaderSearchKey : ISearchKey
    {
        private string name;
        private string value;

        public HeaderSearchKey(ReadOnlySpan<byte> name, ReadOnlySpan<byte> value)
        {
            this.name = name.AsString();
            this.value = value.AsString();
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Header;
        }
        
        public bool Matches(ImapMessage message, int sequenceId)
        {
            return message.Message.Headers[name].Contains(value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
