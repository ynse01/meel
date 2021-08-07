
namespace Meel
{
    public abstract class Mailbox
    {
        protected Mailbox()
        {
            CanWrite = true;
        }

        public uint Uid { get; protected set; }

        public int NumberOfMessages { get; protected set; }

        public int NumberOfRecentMessages { get; protected set; }

        public int FirstUnseenMessage { get; protected set; }

        public bool CanWrite { get; protected set; }

        public bool NoSelect { get; protected set; }

        public abstract void Select();

        public abstract ImapMessage GetMessage(uint sequenceId);

        public abstract uint GetSequenceNumber(ImapMessage message);
    }
}
