using Meel.Responses;
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

        public DataItem Left => left;

        public DataItem Right => right;

        public override void PrintContent(ref ImapResponse response, ImapMessage message)
        {
            left.PrintContent(ref response, message);
            response.AppendSpace();
            right.PrintContent(ref response, message);
        }
    }
}
