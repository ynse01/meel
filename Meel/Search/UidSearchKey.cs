using System;
using System.Collections.Generic;
using Meel.Parsing;

namespace Meel.Search
{
    public class UidSearchKey : ISearchKey
    {
        private ICollection<uint> uids;

        public UidSearchKey(ReadOnlySpan<byte> sequence, uint maxUid)
        {
            uids = SequenceSetParser.Parse(sequence, maxUid);
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.Uid;
        }

        public bool Matches(ImapMessage message, uint sequenceId)
        {
            return uids.Contains(message.Uid);
        }
    }
}
