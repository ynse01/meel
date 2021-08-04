using Meel.Parsing;
using System;
using System.Text;

namespace Meel.DataItems
{
    public unsafe class Rfc822TextDataItem : DataItem
    {
        private static readonly byte[] rfc822Text = Encoding.ASCII.GetBytes("RFC822.TEXT");

        public override ReadOnlySpan<byte> Name => rfc822Text;

        public override void PrintContent(ref Span<byte> span, ImapMessage message)
        {
            Name.CopyTo(span);
            span[Name.Length] = LexiConstants.Space;
            span = span.Slice(Name.Length + 1);
            var body = message.Message.Body;
            SpanStream.StreamTo(ref span, (stream) => body.WriteTo(stream));
        }
    }
}
