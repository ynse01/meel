using System;

namespace Meel
{
    public struct MailboxInfo
    {
        public MailboxInfo(string name, MailboxFlags flags)
        {
            Name = name;
            Flags = flags;
        }

        public string Name { get; }
        public MailboxFlags Flags { get; }
    }
}
