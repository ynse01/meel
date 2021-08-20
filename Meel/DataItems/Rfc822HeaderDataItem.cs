using System;
using System.Text;
using Meel.Parsing;
using Meel.Responses;

namespace Meel.DataItems
{
    public class Rfc822HeaderDataItem : DataItem
    {
        private static readonly byte[] rfc822Header = Encoding.ASCII.GetBytes("RFC822.HEADER");

        public override ReadOnlySpan<byte> Name => rfc822Header;

        public override void PrintContent(ref ImapResponse response, ImapMessage message)
        {
            response.Append(Name);
            response.AppendSpace();
            var size = message.Size.AsSpan();
            response.Append(size);
        }
    }
}
