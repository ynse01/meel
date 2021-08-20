using System;
using Meel.Parsing;
using Meel.Responses;

namespace Meel.DataItems
{
    public class UidDataItem : DataItem
    {
        public override ReadOnlySpan<byte> Name => LexiConstants.Uid;

        public override void PrintContent(ref ImapResponse response, ImapMessage message)
        {
            response.Append(Name);
            response.AppendSpace();
            AppendQuotedString(ref response, message.Uid.AsSpan(), true);
        }
    }
}
