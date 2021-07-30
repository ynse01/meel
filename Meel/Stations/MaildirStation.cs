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

        public bool AppendToMailbox(Mailbox mailbox, ImapMessage message)
        {
            return ((MaildirMailbox)mailbox).AppendMessage(message);
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

        public List<int> ExpungeBySequence(Mailbox mailbox)
        {
            var sequences = ((MaildirMailbox)mailbox).Expunge();
            return sequences;
        }

        public List<int> ExpungeByUid(Mailbox mailbox)
        {
            var mdmb = (MaildirMailbox)mailbox;
            var uids = ExpungeBySequence(mailbox).Select(s => mdmb.Sequence2Uid(s)).ToList();
            return uids;
        }

        public List<string> ListMailboxes(string user, bool subscribed)
        {
            var inbox = Path.Combine(path, user);
            var list = Directory.GetDirectories(inbox).Select(d => Directory.EnumerateDirectories(d).Last()).ToList();
            list.Add("INBOX");
            return list;
        }

        public bool RenameMailbox(string user, string oldName, string newName)
        {
            var oldPath = Path.Combine(path, user, oldName);
            var newPath = Path.Combine(path, user, newName);
            Directory.Move(oldPath, newPath);
            return true;
        }

        public List<int> SearchMailbox(Mailbox mailbox, ISearchKey searchKey)
        {
            return ((MaildirMailbox)mailbox).SearchMessages(searchKey);
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
    }
}
