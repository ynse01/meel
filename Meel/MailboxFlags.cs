using System;

namespace Meel
{
    [Flags]   
    public enum MailboxFlags : byte
    {
        None = 0,
        NoSelect = 1,
        Subscribed = 2,
        ReadOnly = 4
    }
}
