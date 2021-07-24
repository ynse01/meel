using System;
using System.Linq;

namespace Meel.Search
{
    public class BccSearchKey : ISearchKey
    {
        private string needle;

        public BccSearchKey(string needle)
        {
            this.needle = needle;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            var bcc = message.Message.Bcc;
            return bcc.Any(b =>
                b.Name.Contains(needle, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
