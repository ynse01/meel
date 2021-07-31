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

        public override ImapMessage GetMessage(int sequenceId)
        {
            return messages[sequenceId];
        }

        public override int GetSequenceNumber(ImapMessage message)
        {
            return messages.IndexOf(message);
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

        public List<int> Expunge()
        {
            return null;
        }

        public List<int> SearchMessagesBySequence(ISearchKey searchKey)
        {
            var found = new List<int>();
            for(var i = 0; i < messages.Count; i++)
            {
                if (searchKey.Matches(messages[i], i + 1))
                {
                    found.Add(i + 1);
                }
            }
            return found;
        }

        public List<int> SearchMessagesByUid(ISearchKey searchKey)
        {
            var found = new List<int>();
            for (var i = 0; i < messages.Count; i++)
            {
                var message = messages[i];
                if (searchKey.Matches(message, i + 1))
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

        public int Sequence2Uid(int sequenceId)
        {
            return messages[sequenceId].Uid;
        }

        private void UpdateStatistics()
        {
            NumberOfMessages = messages.Count;
            FirstUnseenMessage = 0;
            NumberOfRecentMessages = NumberOfMessages;
        }
    }
}
