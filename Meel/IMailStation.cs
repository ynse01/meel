using System;
using System.Collections.Generic;
using Meel.Search;

namespace Meel
{
    public interface IMailStation : IDisposable
    {
        public Mailbox SelectMailbox(string user, string name);

        public bool CreateMailbox(string user, string name);

        public bool DeleteMailbox(string user, string name);

        public bool RenameMailbox(string user, string oldName, string newName);

        public bool SetSubscription(string user, string name, bool desired);

        public List<MailboxInfo> ListMailboxes(string user, bool subscribed);

        public List<uint> SearchMailbox(Mailbox mailbox, ISearchKey searchKey, bool useSequence);

        public bool AppendToMailbox(Mailbox mailbox, ImapMessage message);

        public bool CopyMessages(IEnumerable<uint> messages, Mailbox source, Mailbox destination);

        public List<uint> ExpungeBySequence(Mailbox mailbox);

        public List<uint> ExpungeByUid(Mailbox mailbox);
    }
}
