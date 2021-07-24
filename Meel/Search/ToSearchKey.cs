using System;
using System.Linq;

namespace Meel.Search
{
    public class ToSearchKey : ISearchKey
    {
        private string needle;

        public ToSearchKey(string needle)
        {
            this.needle = needle;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            var to = message.Message.To;
            return to.Any(b =>
                b.Name.Contains(needle, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
