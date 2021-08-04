using System;
using System.Text;
using Meel.Parsing;
using MimeKit;

namespace Meel.DataItems
{
    public class EnvelopeDataItem : DataItem
    {
        private static readonly byte[] envelope = Encoding.ASCII.GetBytes("ENVELOPE");

        public override ReadOnlySpan<byte> Name => envelope;

        public override void PrintContent(ref Span<byte> span, ImapMessage message)
        {
            Name.CopyTo(span);
            span[Name.Length] = LexiConstants.Space;
            span = span.Slice(Name.Length + 1);
            span[0] = LexiConstants.OpenParenthesis;
            span = span.Slice(1);
            var headers = message.Message.Headers;
            AppendQuotedString(ref span, headers[HeaderId.Date]);
            AppendQuotedString(ref span, headers[HeaderId.Subject]);
            AppendAddress(ref span, headers[HeaderId.From]);
            AppendAddress(ref span, headers[HeaderId.Sender]);
            AppendAddress(ref span, headers[HeaderId.ReplyTo]);
            AppendAddress(ref span, headers[HeaderId.To]);
            AppendAddress(ref span, headers[HeaderId.Cc]);
            AppendAddress(ref span, headers[HeaderId.Bcc]);
            AppendAddress(ref span, headers[HeaderId.InReplyTo]);
            AppendQuotedString(ref span, headers[HeaderId.MessageId], true);
        }

    }
}
