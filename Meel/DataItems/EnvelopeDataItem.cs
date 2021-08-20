using System;
using System.Text;
using Meel.Parsing;
using Meel.Responses;
using MimeKit;

namespace Meel.DataItems
{
    public class EnvelopeDataItem : DataItem
    {
        private static readonly byte[] envelope = Encoding.ASCII.GetBytes("ENVELOPE");

        public override ReadOnlySpan<byte> Name => envelope;

        public override void PrintContent(ref ImapResponse response, ImapMessage message)
        {
            response.Append(Name);
            response.AppendSpace();
            response.Append(LexiConstants.OpenParenthesis);
            var headers = message.Message.Headers;
            AppendQuotedString(ref response, headers[HeaderId.Date]);
            AppendQuotedString(ref response, headers[HeaderId.Subject]);
            AppendAddress(ref response, headers[HeaderId.From]);
            AppendAddress(ref response, headers[HeaderId.Sender]);
            AppendAddress(ref response, headers[HeaderId.ReplyTo]);
            AppendAddress(ref response, headers[HeaderId.To]);
            AppendAddress(ref response, headers[HeaderId.Cc]);
            AppendAddress(ref response, headers[HeaderId.Bcc]);
            AppendAddress(ref response, headers[HeaderId.InReplyTo]);
            AppendQuotedString(ref response, headers[HeaderId.MessageId], true);
        }

    }
}
