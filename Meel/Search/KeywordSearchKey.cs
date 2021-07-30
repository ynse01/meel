using System;
using Meel.Parsing;

namespace Meel.Search
{
    public class KeywordSearchKey : ISearchKey
    {
        private string keyword;
        private bool inverted;

        public KeywordSearchKey(ReadOnlySpan<byte> keyword, bool inverted)
        {
            this.keyword = keyword.AsString().ToUpperInvariant();
            this.inverted = inverted;
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            bool hasFlag = false;
            switch (keyword)
            {
                case "ANSWERED":
                    hasFlag = message.Answered;
                    break;
                case "DELETED":
                    hasFlag = message.Deleted;
                    break;
                case "DRAFT":
                    hasFlag = message.Draft;
                    break;
                case "FLAGGED":
                    hasFlag = message.Flagged;
                    break;
                case "NEW":
                case "RECENT":
                    hasFlag = message.Recent;
                    break;
                case "OLD":
                    hasFlag = !message.Recent;
                    break;
                case "SEEN":
                    hasFlag = message.Seen;
                    break;
            }
            return inverted ^ hasFlag;
        }
    }
}
