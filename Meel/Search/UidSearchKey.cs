using System.Collections.Generic;
using Meel.Parsing;

namespace Meel.Search
{
    public class UidSearchKey : ISearchKey
    {
        private ICollection<string> uids;

        public UidSearchKey(string sequence, string maxUid)
        {
            uids = SequenceSetParser.ParseByUid(sequence, maxUid);
        }

        public bool Matches(ImapMessage message, int sequence)
        {
            return uids.Contains(message.Uid);
        }
    }
}
