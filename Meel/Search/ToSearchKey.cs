using System;
using System.Linq;
using Meel.Parsing;

namespace Meel.Search
{
    public class ToSearchKey : ISearchKey
    {
        private string needle;

        public ToSearchKey(ReadOnlySpan<byte> needle)
        {
            this.needle = needle.AsString();
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Header;
        }
        
        public bool Matches(ImapMessage message, int sequenceId)
        {
            var to = message.Message.To;
            return to.Any(b =>
                b.Name.Contains(needle, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
