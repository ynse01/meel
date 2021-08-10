using Meel.Search;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Meel.Stations
{
    public class InMemoryMailbox : Mailbox
    {
        private List<ImapMessage> messages = new List<ImapMessage>();
        
        public override void Select()
        {
            // Nothing to do here.
        }

        public override ImapMessage GetMessage(uint sequenceId)
        {
            return messages[(int)sequenceId];
        }

        public override uint GetSequenceNumber(ImapMessage message)
        {
            return (uint)messages.IndexOf(message);
        }

        public void SetSubscribed(bool desired)
        {
            Subscribed = desired;
        }

        public bool AppendMessage(ImapMessage message)
        {
            bool result = false;
            if (CanWrite)
            {
                messages.Add(message);
                UpdateStatistics();
                result = true;
            }
            return result;
        }

        public List<uint> Expunge()
        {
            var deleted = new List<uint>();
            for(var i = messages.Count - 1; i >= 0; i--)
            {
                if (messages[i].Deleted)
                {
                    deleted.Add((uint)i);
                    messages.RemoveAt(i);
                }
            }
            return deleted;
        }

        public List<uint> SearchMessagesBySequence(ISearchKey searchKey)
        {
            var found = new List<uint>();
            for(var i = 0; i < messages.Count; i++)
            {
                if (searchKey.Matches(messages[i], (uint)(i + 1)))
                {
                    found.Add((uint)(i + 1));
                }
            }
            return found;
        }

        public List<uint> SearchMessagesByUid(ISearchKey searchKey)
        {
            var found = new List<uint>();
            for (var i = 0; i < messages.Count; i++)
            {
                var message = messages[i];
                if (searchKey.Matches(message, (uint)(i + 1)))
                {
                    found.Add(message.Uid);
                }
            }
            return found;
        }

        public void SetReadonly()
        {
            CanWrite = false;
        }

        public void SetNoSelect(bool desired)
        {
            NoSelect = desired;
        }

        public MailboxFlags GetFlags()
        {
            var flags = MailboxFlags.None;
            if (Subscribed)
            {
                flags |= MailboxFlags.Subscribed;
            }
            if (!CanWrite)
            {
                flags |= MailboxFlags.ReadOnly;
            }
            if (NoSelect)
            {
                flags |= MailboxFlags.NoSelect;
            }
            return flags;
        }

        public uint Sequence2Uid(uint sequenceId)
        {
            return messages[(int)(sequenceId - 1)].Uid;
        }

        private void UpdateStatistics()
        {
            NumberOfMessages = messages.Count;
            FirstUnseenMessage = 0;
            NumberOfRecentMessages = NumberOfMessages;
        }
    }
}
