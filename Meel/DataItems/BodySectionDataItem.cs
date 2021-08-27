using Meel.Parsing;
using Meel.Responses;
using System;

namespace Meel.DataItems
{
    public class BodySectionDataItem : DataItem
    {
        private BodySection section;

        public BodySectionDataItem(BodySection section)
        {
            this.section = section;
        }

        public override ReadOnlySpan<byte> Name
        {
            get
            {
                var inner = section.ToArray();
                var result = new byte[6 + inner.Length];
                Array.Copy(LexiConstants.Body, result, 4);
                result[4] = LexiConstants.SquareOpenBrace;
                Array.Copy(inner, 0, result, 5, inner.Length);
                result[inner.Length + 5] = LexiConstants.SquareCloseBrace;
                return result;
            }
        }

        public override void PrintContent(ref ImapResponse response, ImapMessage message)
        {
            response.Append(Name);
            response.AppendSpace();

            response.Append(LexiConstants.OpenParenthesis);
            BodySectionFormatter.FormatBodySection(message, ref response, section);
            response.Append(LexiConstants.CloseParenthesis);
        }
    }
}
