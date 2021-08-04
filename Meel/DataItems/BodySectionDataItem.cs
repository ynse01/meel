using Meel.Parsing;
using System;

namespace Meel.DataItems
{
    public class BodySectionDataItem : DataItem
    {
        public override ReadOnlySpan<byte> Name => LexiConstants.Body;

        public override void PrintContent(ref Span<byte> span, ImapMessage message)
        {
            Name.CopyTo(span);
            span[Name.Length] = LexiConstants.Space;
            span = span.Slice(Name.Length + 1);

            // TODO: Implement
        }
    }
}
