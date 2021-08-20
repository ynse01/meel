using Meel.Parsing;
using Meel.Responses;
using System;
using System.Text;

namespace Meel.DataItems
{
    public class InternalDateDataItem : DataItem
    {
        private static readonly byte[] internalDate = Encoding.ASCII.GetBytes("INTERNALDATE");

        public override ReadOnlySpan<byte> Name => internalDate;

        public override void PrintContent(ref ImapResponse response, ImapMessage message)
        {
            response.Append(Name);
            response.AppendSpace();
            response.Append(LexiConstants.DoubleQuote);
            Rfc822Formatter.TryFormat(message.InternalDate, ref response);
            response.Append(LexiConstants.DoubleQuote);
        }
    }
}
