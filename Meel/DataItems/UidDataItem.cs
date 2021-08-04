using System;
using Meel.Parsing;

namespace Meel.DataItems
{
    public class UidDataItem : DataItem
    {
        public override ReadOnlySpan<byte> Name => LexiConstants.Uid;

        public override void PrintContent(ref Span<byte> span, ImapMessage message)
        {
            Name.CopyTo(span);
            span[Name.Length] = LexiConstants.Space;
            span = span.Slice(Name.Length + 1);
            AppendQuotedString(ref span, message.Uid.AsSpan(), true);
        }
    }
}
