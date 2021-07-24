using System;

namespace Meel
{
    [Flags]   
    public enum MessageFlags : byte
    {
        None = 0,
        Draft = 1,
        Read = 2,
        Seen = 4,
        Trashed = 8,
        Flagged = 16,
        Answered = 32
    }
}
