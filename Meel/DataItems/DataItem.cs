using Meel.Parsing;
using MimeKit;
using System;

namespace Meel.DataItems
{
    public abstract class DataItem
    {
        public abstract ReadOnlySpan<byte> Name { get; }

        public abstract void PrintContent(ref Span<byte> span, ImapMessage message);

        protected void AppendQuotedString(ref Span<byte> span, string content, bool appendSpace = true)
        {
            AppendQuotedString(ref span, content.AsAsciiSpan(), appendSpace);
        }

        protected void AppendQuotedString(ref Span<byte> span, Span<byte> content, bool appendSpace = true)
        {
            span[0] = LexiConstants.DoubleQuote;
            span = span.Slice(1);
            content.CopyTo(span);
            var contentLength = content.Length;
            span[contentLength] = LexiConstants.DoubleQuote;
            if (appendSpace)
            {
                span[contentLength + 1] = LexiConstants.Space;
                span = span.Slice(contentLength + 2);
            }
            else
            {
                span = span.Slice(contentLength + 1);
            }
        }

        protected void AppendAddress(ref Span<byte> span, string value)
        {
            var addresses = InternetAddressList.Parse(value);
            if (addresses.Count == 0)
            {
                LexiConstants.Nil.CopyTo(span);
                span[3] = LexiConstants.Space;
                span = span.Slice(4);
            }
            else
            {
                Rfc822Formatter.TryFormat(addresses, span, out int bytesWritten);
                span[bytesWritten] = LexiConstants.Space;
                span = span.Slice(bytesWritten + 1);
            }
        }

    }
}
