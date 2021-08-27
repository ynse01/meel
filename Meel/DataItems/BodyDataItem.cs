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
            var disposition = message.Message.Body?.ContentDisposition;
            if (disposition != null)
            {
                var parameters = disposition.Parameters;
                for (var i = 0; i < parameters.Count; i++)
                {
                    var parameter = parameters[i];
                    AppendQuotedString(ref response, parameter.Name);
                    AppendQuotedString(ref response, parameter.Value, (i + 1) != parameters.Count);
                }
            }
            else
            {
                response.Append(LexiConstants.Nil);
            }
            response.Append(LexiConstants.CloseParenthesis);
            response.AppendSpace();

            if (disposition != null)
            {
                AppendQuotedString(ref response, disposition.Disposition);
            } else
            {
                response.Append(LexiConstants.Nil);
            }
            AppendQuotedString(ref response, message.Message.Headers[MimeKit.HeaderId.Language]);
            var location = message.Message.Body?.ContentLocation;
            if (location != null)
            {
                AppendQuotedString(ref response, location.ToString(), false);
            } else
            {
                response.Append(LexiConstants.Nil);
            }
            response.Append(LexiConstants.CloseParenthesis);
        }
    }
}
