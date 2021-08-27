
using Meel.DataItems;
using MimeKit;
using System;

namespace Meel.Responses
{
    public static class BodySectionFormatter
    {
        public static void FormatBodySection(ImapMessage message, ref ImapResponse response, BodySection section)
        {
            var part = FetchMessagePart(message.Message.Body, section.Parts);
            switch(section.Subset)
            {
                case BodySubset.Header:
                    FormatHeader(part, ref response);
                    break;
                case BodySubset.HeaderFields:
                    FormatFields(part, ref response);
                    break;
                case BodySubset.HeaderFieldsNot:
                    FormatFieldsNot(part, ref response);
                    break;
                case BodySubset.Mime:
                    FormatMime(part, ref response);
                    break;
                case BodySubset.Text:
                    FormatText(part, ref response);
                    break;
            }
        }

        private static MimeEntity FetchMessagePart(MimeEntity entity, ReadOnlySpan<int> parts)
        {
            var multipart = entity as Multipart;
            if (parts.Length == 0 || multipart == null)
            {
                return entity;
            }
            var index = parts[0];
            parts = parts.Slice(1);
            var next = multipart[index];
            return FetchMessagePart(next, parts);
        }

        private static void FormatHeader(MimeEntity part, ref ImapResponse response) { }
        private static void FormatFields(MimeEntity part, ref ImapResponse response) { }
        private static void FormatFieldsNot(MimeEntity part, ref ImapResponse response) { }
        private static void FormatMime(MimeEntity part, ref ImapResponse response) { }
        private static void FormatText(MimeEntity part, ref ImapResponse response) 
        {
            var textPart = part as TextPart;
            response.Append(textPart.Text);
        }
    }
}
