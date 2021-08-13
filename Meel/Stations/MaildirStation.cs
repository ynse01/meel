using Meel.Search;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Meel.Stations
{
    public class MaildirStation : IMailStation
    {
        private readonly string path;

        public MaildirStation(string path)
        {
            this.path = path;
        }

        public void Dispose()
        {
            // Nothig to do here.
        }

        public bool AppendToMailbox(Mailbox mailbox, ImapMessage message)
        {
            return ((MaildirMailbox)mailbox).AppendMessage(message);
        }

        public bool CopyMessages(IEnumerable<uint> messages, Mailbox source, Mailbox destination)
        {
            // TODO: Implement
            return true;
        }

        public bool CreateMailbox(string user, string name)
        {
            var newPath = Path.Combine(path, user, name);
            Directory.CreateDirectory(newPath);
            return MaildirMailbox.Create(newPath) != null;
        }

        public bool DeleteMailbox(string user, string name)
        {
            var newPath = Path.Combine(path, user, name);
            Directory.Delete(newPath);
            return true;
        }

        public List<uint> ExpungeBySequence(Mailbox mailbox)
        {
            var sequences = ((MaildirMailbox)mailbox).Expunge();
            return sequences;
        }

        public List<uint> ExpungeByUid(Mailbox mailbox)
        {
            var mdmb = (MaildirMailbox)mailbox;
            var uids = ExpungeBySequence(mailbox).Select(s => mdmb.Sequence2Uid(s)).ToList();
            return uids;
        }

        public List<MailboxInfo> ListMailboxes(string user, bool subscribed)
        {
            var inbox = Path.Combine(path, user);
            var list = Directory.GetDirectories(inbox).Select(d => Directory.EnumerateDirectories(d).Last()).ToList();
            list.Add("INBOX");
            return list.Select(name => new MailboxInfo(name, MailboxFlags.None)).ToList();
        }

        public bool RenameMailbox(string user, string oldName, string newName)
        {
            var oldPath = Path.Combine(path, user, oldName);
            var newPath = Path.Combine(path, user, newName);
            Directory.Move(oldPath, newPath);
            return true;
        }

        public List<uint> SearchMailbox(Mailbox mailbox, ISearchKey searchKey, bool useSequence)
        {
            List<uint> list;
            if (useSequence)
            {
                list = ((MaildirMailbox)mailbox).SearchMessagesBySequence(searchKey);
            } else
            {
                list = ((MaildirMailbox)mailbox).SearchMessagesByUid(searchKey);
            }
            return list;
        }

        public Mailbox SelectMailbox(string user, string name)
        {
            var selected = Path.Combine(path, user, name);
            var mailbox = new MaildirMailbox(selected);
            mailbox.Select();
            return mailbox;
        }

        public bool SetSubscription(string user, string name, bool desired)
        {
            // TODO: Implement
            return false;
        }

        public MailboxFlags? GetMailboxFlags(string user, string name)
        {
            // TODO: Implement
            return null;
        }

    }
}
