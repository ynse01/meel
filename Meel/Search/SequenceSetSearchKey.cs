using System;
using System.Collections.Generic;
using Meel.Parsing;

namespace Meel.Search
{
    public class SequenceSetSearchKey : ISearchKey
    {
        private ICollection<int> list;

        public SequenceSetSearchKey(ReadOnlySpan<byte> sequence, int maxId)
        {
            list = SequenceSetParser.Parse(sequence, maxId);
        }

        public SearchDepth GetSearchDepth()
        {
            return SearchDepth.None;
        }

        public bool Matches(ImapMessage message, int sequenceId)
        {
            return list.Contains(sequenceId);
        }
    }
}
