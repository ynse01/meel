using System;
using System.Text;
using Meel.Parsing;

namespace Meel.DataItems
{
    public class Rfc822SizeDataItem : DataItem
    {
        private static readonly byte[] rfc822Size = Encoding.ASCII.GetBytes("RFC822.SIZE");

        public override ReadOnlySpan<byte> Name => rfc822Size;

        public override void PrintContent(ref Span<byte> span, ImapMessage message)
        {
            Name.CopyTo(span);
            span[Name.Length] = LexiConstants.Space;
            span = span.Slice(Name.Length + 1);
            var size = message.Size.AsSpan();
            size.CopyTo(span);
            span = span.Slice(size.Length);
        }
    }
}
