using System;
using System.Linq;
using Meel.Parsing;

namespace Meel.Search
{
    public class CcSearchKey : ISearchKey
    {
        private string needle;

        public CcSearchKey(ReadOnlySpan<byte> needle)
        {
            this.needle = needle.AsString();
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            var cc = message.Message.Cc;
            return cc.Any(b =>
                b.Name.Contains(needle, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
