using System;

namespace Meel.Search
{
    public interface ISearchKey
    {
        bool Matches(ImapMessage message, int sequence);
    }
}
