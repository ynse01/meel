using Meel.Parsing;
using System;
using System.Text;

namespace Meel.DataItems
{
    public class InternalDateDataItem : DataItem
    {
        private static readonly byte[] internalDate = Encoding.ASCII.GetBytes("INTERNALDATE");

        public override ReadOnlySpan<byte> Name => internalDate;

        public override void PrintContent(ref Span<byte> span, ImapMessage message)
        {
            Name.CopyTo(span);
            span[Name.Length] = LexiConstants.Space;
            span = span.Slice(Name.Length + 1);
            span[0] = LexiConstants.DoubleQuote;
            span = span.Slice(1);
            Rfc822Formatter.TryFormat(message.InternalDate, span, out int bytesWritten);
            span[bytesWritten] = LexiConstants.DoubleQuote;
            span = span.Slice(bytesWritten + 1);
        }
    }
}
