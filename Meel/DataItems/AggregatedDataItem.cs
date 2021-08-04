using Meel.Parsing;
using System;

namespace Meel.DataItems
{
    public class AggregatedDataItem : DataItem
    {
        private DataItem left;
        private DataItem right;

        public AggregatedDataItem(DataItem left, DataItem right)
        {
            this.left = left;
            this.right = right;
        }

        public override ReadOnlySpan<byte> Name => new byte[0];

        public override void PrintContent(ref Span<byte> span, ImapMessage message)
        {
            var leftName = left.Name;
            leftName.CopyTo(span);
            span[leftName.Length] = LexiConstants.Space;
            span = span.Slice(leftName.Length + 1);
            left.PrintContent(ref span, message);
            span[0] = LexiConstants.Space;
            var rightName = right.Name;
            rightName.CopyTo(span);
            span[rightName.Length] = LexiConstants.Space;
            span = span.Slice(rightName.Length + 1);
            right.PrintContent(ref span, message);
        }
    }
}
