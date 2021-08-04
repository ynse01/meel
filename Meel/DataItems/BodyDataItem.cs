using Meel.Parsing;
using System;

namespace Meel.DataItems
{
    /// <summary>
    /// Body date item consists of (in order):
    /// - Body parameter list
    /// - Body disposition
    /// - Body language
    /// - Body location
    /// </summary>
    public class BodyDataItem : DataItem
    {
        public override ReadOnlySpan<byte> Name => LexiConstants.Body;

        public override void PrintContent(ref Span<byte> span, ImapMessage message)
        {
            Name.CopyTo(span);
            span[Name.Length] = LexiConstants.Space;
            span = span.Slice(Name.Length + 1);

            // TODO: What are message parameters ?
            span[0] = LexiConstants.OpenParenthesis;
            var parameters = message.Message.Body.ContentDisposition.Parameters;
            for(var i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                AppendQuotedString(ref span, parameter.Name);
                AppendQuotedString(ref span, parameter.Value, (i + 1) != parameters.Count);
            }
            span[0] = LexiConstants.CloseParenthesis;
            span[1] = LexiConstants.Space;
            span = span.Slice(2);

            AppendQuotedString(ref span, message.Message.Body.ContentDisposition.Disposition);
            AppendQuotedString(ref span, message.Message.Headers[MimeKit.HeaderId.Language]);
            AppendQuotedString(ref span, message.Message.Body.ContentLocation.ToString(), false);
            span[0] = LexiConstants.CloseParenthesis;
            span = span.Slice(1);
        }
    }
}
