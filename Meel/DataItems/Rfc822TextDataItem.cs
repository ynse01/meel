using Meel.Parsing;
using Meel.Responses;
using System;
using System.Text;

namespace Meel.DataItems
{
    public unsafe class Rfc822TextDataItem : DataItem
    {
        private static readonly byte[] rfc822Text = Encoding.ASCII.GetBytes("RFC822.TEXT");

        public override ReadOnlySpan<byte> Name => rfc822Text;

        public override void PrintContent(ref ImapResponse response, ImapMessage message)
        {
            response.Append(Name);
            response.AppendSpace();
            var body = message.Message.Body;
            using (var stream = response.GetStream())
            {
                body.WriteTo(stream);
            }
        }
    }
}
