using System;
using System.Linq;

namespace Meel.Search
{
    public class CcSearchKey : ISearchKey
    {
        private string needle;

        public CcSearchKey(string needle)
        {
            this.needle = needle;
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
