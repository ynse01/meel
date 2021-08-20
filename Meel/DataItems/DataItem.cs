using Meel.Parsing;
using Meel.Responses;
using MimeKit;
using System;

namespace Meel.DataItems
{
    public abstract class DataItem
    {
        public abstract ReadOnlySpan<byte> Name { get; }

        public abstract void PrintContent(ref ImapResponse response, ImapMessage message);

        protected void AppendQuotedString(ref ImapResponse response, string content, bool appendSpace = true)
        {
            AppendQuotedString(ref response, content.AsAsciiSpan(), appendSpace);
        }

        protected void AppendQuotedString(ref ImapResponse response, Span<byte> content, bool appendSpace = true)
        {
            response.Append(LexiConstants.DoubleQuote);
            response.Append(content);
            response.Append(LexiConstants.DoubleQuote);
            if (appendSpace)
            {
                response.AppendSpace();
            }
        }

        protected void AppendAddress(ref ImapResponse response, string value)
        {
            if (!string.IsNullOrEmpty(value)) {
                var addresses = InternetAddressList.Parse(value);
                Rfc822Formatter.TryFormat(addresses, ref response);
                response.AppendSpace();
            } else
            {
                response.Append(LexiConstants.Nil);
                response.AppendSpace();
            }
        }
    }
}
