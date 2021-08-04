using Meel.Parsing;
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

        public override void PrintContent(ref Span<byte> span, ImapMessage message)
        {
            Name.CopyTo(span);
            span[Name.Length] = LexiConstants.Space;
            span = span.Slice(Name.Length + 1);

            // TODO: Implement
        }
    }
}
