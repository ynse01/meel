using Meel.Search;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Meel.Stations
{
    public class InMemoryStation : IMailStation
    {
        private Dictionary<string, InMemoryMailbox> mailboxes;

        public InMemoryStation()
        {
            mailboxes = new Dictionary<string, InMemoryMailbox>(StringComparer.OrdinalIgnoreCase);
        }

        public bool CreateMailbox(string user, string name)
        {
            bool result = false;
            var boxName = user + ":" + name;
            if (!mailboxes.ContainsKey(boxName))
            {
                mailboxes.Add(boxName, new InMemoryMailbox());
                result = true;
            }
            return result;
        }

        public bool DeleteMailbox(string user, string name)
        {
            bool result = false;
            var boxName = user + ":" + name;
            if (mailboxes.ContainsKey(boxName))
            {
                mailboxes.Remove(boxName);
                result = true;
            }
            return result;
        }

        public bool AppendToMailbox(Mailbox mailbox, ImapMessage message)
        {
            return ((InMemoryMailbox)mailbox).AppendMessage(message);
        }

        public List<int> ExpungeBySequence(Mailbox mailbox)
        {
            return ((InMemoryMailbox)mailbox).Expunge();
        }

        public List<string> ExpungeByUid(Mailbox mailbox)
        {
            var immb = (InMemoryMailbox)mailbox;
            return immb.Expunge().Select(s => immb.Sequence2Uid(s)).ToList();
        }

        public List<string> ListMailboxes(string user, bool subscribed)
        {
            return mailboxes.Keys.Where(name => name.StartsWith(user)).ToList();
        }

        public List<int> SearchMailbox(Mailbox mailbox, ISearchKey searchKey) 
        {
            var immb = (InMemoryMailbox)mailbox;
            return immb.SearchMessages(searchKey);
        }

        public bool RenameMailbox(string user, string oldName, string newName)
        {
            var result = false;
            var oldBoxName = GetBoxName(user, oldName);
            var newBoxName = GetBoxName(user, newName);
            if (mailboxes.ContainsKey(oldBoxName) && !mailboxes.ContainsKey(newBoxName))
            {
                mailboxes[newBoxName] = mailboxes[oldBoxName];
                mailboxes.Remove(oldName);
                result = true;
            }
            return result;
        }

        public Mailbox SelectMailbox(string user, string name)
        {
            var boxName = GetBoxName(user, name);
            mailboxes.TryGetValue(boxName, out InMemoryMailbox mailbox);
            return mailbox;
        }

        public bool SetSubscription(string user, string name, bool desired)
        {
            bool result = false;
            var boxName = GetBoxName(user, name);
            if (mailboxes.ContainsKey(boxName))
            {
                mailboxes[boxName].Subscribed = desired;
                result = true;
            }
            return result;
        }

        private string GetBoxName(string user, string name)
        {
            return user + ":" + name;
        }
    }
}
