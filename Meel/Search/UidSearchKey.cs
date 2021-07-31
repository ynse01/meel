using System;
using System.Collections.Generic;
using Meel.Parsing;

namespace Meel.Search
{
    public class UidSearchKey : ISearchKey
    {
        private ICollection<int> uids;

        public UidSearchKey(ReadOnlySpan<byte> sequence, int maxUid)
        {
            uids = SequenceSetParser.Parse(sequence, maxUid);
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Uid;
        }

        public bool Matches(ImapMessage message, int sequenceId)
        {
            return uids.Contains(message.Uid);
        }
    }
}
