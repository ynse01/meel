using System;
using System.Linq;

namespace Meel.Search
{
    public class FromSearchKey : ISearchKey
    {
        private string needle;

        public FromSearchKey(string needle)
        {
            this.needle = needle;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            var from = message.Message.From;
            return from.Any(b =>
                b.Name.Contains(needle, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
