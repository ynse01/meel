using System;
using System.Collections.Generic;
using Meel.Parsing;

namespace Meel.Search
{
    public class SequenceSetSearchKey : ISearchKey
    {
        private ICollection<uint> list;

        public SequenceSetSearchKey(ReadOnlySpan<byte> sequence, uint maxId)
        {
            list = SequenceSetParser.Parse(sequence, maxId);
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.None;
        }

        public bool Matches(ImapMessage message, uint sequenceId)
        {
            return list.Contains(sequenceId);
        }
    }
}
