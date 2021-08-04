using Meel.Parsing;
using System;
using System.Text;

namespace Meel.DataItems
{
    public class FlagsDataItem : DataItem
    {
        private static readonly byte[] flags = Encoding.ASCII.GetBytes("FLAGS");
        private static readonly byte[] answered = Encoding.ASCII.GetBytes(@"\Answered");
        private static readonly byte[] deleted = Encoding.ASCII.GetBytes(@"\Deleted");
        private static readonly byte[] draft = Encoding.ASCII.GetBytes(@"\Draft");
        private static readonly byte[] flagged = Encoding.ASCII.GetBytes(@"\Flagged");
        private static readonly byte[] recent = Encoding.ASCII.GetBytes(@"\Recent");
        private static readonly byte[] seen = Encoding.ASCII.GetBytes(@"\Seen");

        public override ReadOnlySpan<byte> Name => flags;

        public override void PrintContent(ref Span<byte> span, ImapMessage message)
        {
            Name.CopyTo(span);
            span[Name.Length] = LexiConstants.Space;
            span = span.Slice(Name.Length + 1);
            if (message.Answered)
            {
                answered.CopyTo(span);
                span = span.Slice(answered.Length);
            }
            if (message.Deleted)
            {
                deleted.CopyTo(span);
                span = span.Slice(deleted.Length);
            }
            if (message.Draft)
            {
                draft.CopyTo(span);
                span = span.Slice(draft.Length);
            }
            if (message.Flagged)
            {
                flagged.CopyTo(span);
                span = span.Slice(flagged.Length);
            }
            if (message.Recent)
            {
                recent.CopyTo(span);
                span = span.Slice(recent.Length);
            }
            if (message.Seen)
            {
                seen.CopyTo(span);
                span = span.Slice(seen.Length);
            }
        }
    }
}
