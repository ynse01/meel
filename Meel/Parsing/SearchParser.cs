using Meel.Search;
using System;
using System.Text;

namespace Meel.Parsing
{
    public static class SearchParser
    {
        public static ISearchKey Parse(string query)
        {
            var buffer = Encoding.ASCII.GetBytes(query);
            ReadOnlySpan<byte> span = buffer;
            return Parse(span);
        }

        public static ISearchKey Parse(ReadOnlySpan<byte> query)
        {
            var stop = query.IndexOf(LexiConstants.Space);
            return new AllSearchKey();
        }
    }
}
