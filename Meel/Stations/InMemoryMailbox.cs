using Meel.Search;
using System;
using System.Collections.Generic;

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

        public bool Subscribed { get; set; }

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
            return null;
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
