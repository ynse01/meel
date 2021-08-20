using Meel.Parsing;
using Meel.Responses;
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

        public override void PrintContent(ref ImapResponse response, ImapMessage message)
        {
            response.Append(Name);
            response.AppendSpace();

            // TODO: What are message parameters ?
            response.Append(LexiConstants.OpenParenthesis);
            var parameters = message.Message.Body.ContentDisposition.Parameters;
            for(var i = 0; i < parameters.Count; i++)
            {
                var parameter = parameters[i];
                AppendQuotedString(ref response, parameter.Name);
                AppendQuotedString(ref response, parameter.Value, (i + 1) != parameters.Count);
            }
            response.Append(LexiConstants.CloseParenthesis);
            response.AppendSpace();
            
            AppendQuotedString(ref response, message.Message.Body.ContentDisposition.Disposition);
            AppendQuotedString(ref response, message.Message.Headers[MimeKit.HeaderId.Language]);
            AppendQuotedString(ref response, message.Message.Body.ContentLocation.ToString(), false);
            response.Append(LexiConstants.CloseParenthesis);
        }
    }
}
