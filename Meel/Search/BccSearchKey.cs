using System;
using System.Linq;
using Meel.Parsing;

namespace Meel.Search
{
    public class BccSearchKey : ISearchKey
    {
        private string value;

        public BccSearchKey(ReadOnlySpan<byte> value)
        {
            this.value = value.AsString();
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            var bcc = message.Message.Bcc;
            return bcc.Any(b =>
                b.Name.Contains(value, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
