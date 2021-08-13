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

        public void Dispose()
        {
            // Nothig to do here.
        }

        public bool CreateMailbox(string user, string name)
        {
            bool result = false;
            var boxName = GetBoxName(user, name);
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

        public bool CopyMessages(IEnumerable<uint> sequenceSet, Mailbox source, Mailbox destination)
        {
            bool result = true;
            foreach (var sequence in sequenceSet)
            {
                var message = ((InMemoryMailbox)source).GetMessage(sequence);
                if (message != null)
                {
                    ((InMemoryMailbox)destination).AppendMessage(message);
                } else
                {
                    // TODO: Rollback previous copied messages.
                    result = false;
                    break;
                }
            }
            return result;
        }

        public List<uint> ExpungeBySequence(Mailbox mailbox)
        {
            return ((InMemoryMailbox)mailbox).Expunge();
        }

        public List<uint> ExpungeByUid(Mailbox mailbox)
        {
            var immb = (InMemoryMailbox)mailbox;
            return immb.Expunge().Select(s => immb.Sequence2Uid(s)).ToList();
        }

        public List<MailboxInfo> ListMailboxes(string user, bool subscribed)
        {
            return mailboxes
                .Select(pair => new MailboxInfo(pair.Key, pair.Value.GetFlags()))
                .Where(info => info.Name.StartsWith(user))
                .ToList();
        }

        public List<uint> SearchMailbox(Mailbox mailbox, ISearchKey searchKey, bool useSequence) 
        {
            var immb = (InMemoryMailbox)mailbox;
            List<uint> list;
            if (useSequence)
            {
                list = immb.SearchMessagesBySequence(searchKey);
            } else
            {
                list = immb.SearchMessagesByUid(searchKey);
            }
            return list;
        }

        public bool RenameMailbox(string user, string oldName, string newName)
        {
            var result = false;
            var oldBoxName = GetBoxName(user, oldName);
            var newBoxName = GetBoxName(user, newName);
            if (mailboxes.ContainsKey(oldBoxName) && !mailboxes.ContainsKey(newBoxName))
            {
                mailboxes[newBoxName] = mailboxes[oldBoxName];
                mailboxes.Remove(oldBoxName);
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
                ((InMemoryMailbox)mailboxes[boxName]).SetSubscribed(desired);
                result = true;
            }
            return result;
        }

        public MailboxFlags? GetMailboxFlags(string user, string name)
        {
            var mailbox = SelectMailbox(user, name);
            if (mailbox != null)
            {
                return ((InMemoryMailbox)mailbox).GetFlags();
            } else
            {
                return null;
            }
        }

        private string GetBoxName(string user, string name)
        {
            return user + ":" + name;
        }
    }
}
