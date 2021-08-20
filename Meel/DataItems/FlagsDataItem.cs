using Meel.Parsing;
using Meel.Responses;
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

        public override void PrintContent(ref ImapResponse response, ImapMessage message)
        {
            response.Append(Name);
            response.AppendSpace();
            response.Append(LexiConstants.OpenParenthesis);
            var before = response.Length;
            if (message.Answered)
            {
                response.Append(answered);
                response.AppendSpace();
            }
            if (message.Deleted)
            {
                response.Append(deleted);
                response.AppendSpace();
            }
            if (message.Draft)
            {
                response.Append(draft);
                response.AppendSpace();
            }
            if (message.Flagged)
            {
                response.Append(flagged);
                response.AppendSpace();
            }
            if (message.Recent)
            {
                response.Append(recent);
                response.AppendSpace();
            }
            if (message.Seen)
            {
                response.Append(seen);
                response.AppendSpace();
            }
            if (response.Length != before)
            {
                response.Rewind(1);
            }
            response.Append(LexiConstants.CloseParenthesis);
        }
    }
}
