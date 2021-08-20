using System;
using System.Text;
using Meel.Parsing;
using Meel.Responses;

namespace Meel.DataItems
{
    public class Rfc822SizeDataItem : DataItem
    {
        private static readonly byte[] rfc822Size = Encoding.ASCII.GetBytes("RFC822.SIZE");

        public override ReadOnlySpan<byte> Name => rfc822Size;

        public override void PrintContent(ref ImapResponse response, ImapMessage message)
        {
            response.Append(Name);
            response.AppendSpace();
            var size = message.Size.AsSpan();
            response.Append(size);
        }
    }
}
