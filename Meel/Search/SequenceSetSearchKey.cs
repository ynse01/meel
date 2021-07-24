using System;
using System.Collections.Generic;
using Meel.Parsing;

namespace Meel.Search
{
    public class SequenceSetSearchKey : ISearchKey
    {
        private ICollection<int> list;

        public SequenceSetSearchKey(string sequence, int maxId)
        {
            list = SequenceSetParser.ParseBySequenceId(sequence, maxId);
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return list.Contains(sequence);
        }
    }
}
