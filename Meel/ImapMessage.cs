using MimeKit;

namespace Meel
{
    public class ImapMessage
    {
        public ImapMessage(MimeMessage message, int uid, MessageFlags flags, long size)
        {
            Message = message;
            Uid = uid;
            Seen = flags.HasFlag(MessageFlags.Seen);
            Deleted = flags.HasFlag(MessageFlags.Trashed);
            Flagged = flags.HasFlag(MessageFlags.Flagged);
            Draft = flags.HasFlag(MessageFlags.Draft);
            Recent = flags.HasFlag(MessageFlags.Recent);
            Size = size;
        }

        public int Uid { get; private set; }

        public long Size { get; private set; }

        public MimeMessage Message { get; private set; }

        public bool Seen { get; set; }

        public bool Deleted { get; set; }

        public bool Flagged { get; set; }

        public bool Answered { get; set; }

        public bool Draft { get; set; }

        public bool Recent { get; set; }
    }
}
