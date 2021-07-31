using System;
using System.Linq;
using Meel.Parsing;

namespace Meel.Search
{
    public class FromSearchKey : ISearchKey
    {
        private string needle;

        public FromSearchKey(ReadOnlySpan<byte> needle)
        {
            this.needle = needle.AsString();
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Header;
        }
        
        public bool Matches(ImapMessage message, uint sequenceId)
        {
            var from = message.Message.From;
            return from.Any(b =>
                b.Name.Contains(needle, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
