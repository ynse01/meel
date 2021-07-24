using MimeKit;

namespace Meel
{
    public abstract class Mailbox
    {
        public Mailbox()
        {
            CanWrite = true;
        }

        public int NumberOfMessages { get; protected set; }

        public int NumberOfRecentMessages { get; protected set; }

        public int FirstUnseenMessage { get; protected set; }

        public bool CanWrite { get; protected set; }

        public abstract void Select();

        public abstract ImapMessage GetMessage(int sequenceId);

        public abstract int GetSequenceNumber(ImapMessage message);
    }
}
