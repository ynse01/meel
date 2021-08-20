using Meel.Parsing;
using Meel.Responses;
using System;

namespace Meel.DataItems
{
    public class BodySectionDataItem : DataItem
    {
        public override ReadOnlySpan<byte> Name => LexiConstants.Body;

        public override void PrintContent(ref ImapResponse response, ImapMessage message)
        {
            response.Append(Name);
            response.AppendSpace();
            
            // TODO: Implement
        }
    }
}
