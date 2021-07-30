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

        public bool Matches(ImapMessage message, int sequence)
        {
            return list.Contains(sequence);
        }
    }
}
