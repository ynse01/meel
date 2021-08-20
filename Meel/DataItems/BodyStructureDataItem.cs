using Meel.Parsing;
using Meel.Responses;
using System;

namespace Meel.DataItems
{
    /// <summary>
    /// BodyStructure date item consists of (in order):
    /// - Body parameter list
    /// - Body disposition
    /// - Body language
    /// - Body location
    /// - Extension data
    /// </summary>
    public class BodyStructureDataItem : DataItem
    {
        public override ReadOnlySpan<byte> Name => LexiConstants.BodyStructure;

        public override void PrintContent(ref ImapResponse response, ImapMessage message)
        {
            response.Append(Name);
            response.AppendSpace();
            
            // TODO: Implement
        }
    }
}
