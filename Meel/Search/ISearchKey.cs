using System;

namespace Meel.Search
{
    [Flags]
    public enum SearchDepth
    {
        None = 0,
        Flags = 1,
        Size = 2,
        Uid = 4,
        Header = 8,
        Body = 16
    }

    public interface ISearchKey
    {
        SearchDepth GetSearchDepth();

        bool Matches(ImapMessage message, int sequenceId);
    }
}
