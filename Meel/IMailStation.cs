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

        public List<string> ListMailboxes(string user, bool subscribed);

        public List<int> SearchMailbox(Mailbox mailbox, ISearchKey searchKey);

        public bool AppendToMailbox(Mailbox mailbox, ImapMessage message);

        public List<int> ExpungeBySequence(Mailbox mailbox);

        public List<int> ExpungeByUid(Mailbox mailbox);
    }
}
