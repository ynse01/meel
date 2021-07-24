using Meel.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Meel.Stations
{
    public class MaildirMailbox : Mailbox
    {
        private static long numMessagesDelivered = 0L;
        
        private string path;
        private List<string> files;

        public static Mailbox Create(string path)
        {
            Directory.CreateDirectory(Path.Combine(path, "new"));
            Directory.CreateDirectory(Path.Combine(path, "cur"));
            Directory.CreateDirectory(Path.Combine(path, "tmp"));
            return new MaildirMailbox(path);
        }

        public MaildirMailbox(string path)
        {
            this.path = path;
        }

        public override void Select()
        {
            files = new List<string>();
            SyncWithDisk();
        }

        public override ImapMessage GetMessage(int sequenceId)
        {
            var filename = files[sequenceId];
            using (var stream = new FileStream(filename, FileMode.Open))
            {
                var message = MimeMessage.Load(stream);
                var flags = ParseFlags(filename);
                return new ImapMessage(message, filename, flags, stream.Length);
            }
        }

        public override int GetSequenceNumber(ImapMessage message)
        {
            // TODO: Implement
            return -1;
        }

        public bool AppendMessage(ImapMessage message)
        {
            var name = GenerateUniqueFilename();
            var tmpPath = Path.Combine(path, "tmp", name);
            using (var stream = new FileStream(tmpPath, FileMode.CreateNew))
            {
                message.Message.WriteTo(stream);
            }
            MoveToNew(tmpPath);
            return true;
        }

        public List<int> Expunge()
        {
            var deleted = new List<int>();
            // TODO: Implement
            return deleted;
        }

        public List<int> SearchMessages(ISearchKey searchKey)
        {
            // TODO: Implement
            return null;
        }

        public string Sequence2Uid(int sequenceId)
        {
            return files[sequenceId];
        }

        private void SyncWithDisk()
        {
            var curPaths = Directory.GetFiles(Path.Combine(path, "cur"));
            files.AddRange(curPaths);
            var newPaths = Directory.GetFiles(Path.Combine(path, "new"));
            foreach(var newFile in newPaths)
            {
                var curFile = newFile.Replace(@"\new\", @"\cur\");
                File.Move(newFile, curFile);
                files.Add(curFile);
            }
        }

        private string GenerateUniqueFilename()
        {
            var time = DateTime.Now.ToFileTimeUtc();
            var pid = Process.GetCurrentProcess().Id;
            var host = Environment.MachineName;
            var num = Interlocked.Increment(ref numMessagesDelivered);
            return $"{time}.M{time}P{pid}Q{num}.{host}";
        }

        private void MoveToNew(string tmpFile)
        {
            if (File.Exists(tmpFile))
            {
                var size = new FileInfo(tmpFile).Length;
                var curFile = tmpFile.Replace("\\tmp\\", "\\cur\\") + $",S={size};2,";
                if (!File.Exists(curFile))
                {
                    File.Move(tmpFile, curFile);
                }
            }
        }

        private MessageFlags ParseFlags(string filename)
        {
            MessageFlags flags = MessageFlags.None;
            bool stop = false;
            for(var i = filename.Length - 1; stop; i++)
            {
                var chr = filename[i];
                switch(chr)
                {
                    case 'D':
                    case 'd':
                        flags |= MessageFlags.Draft;
                        break;
                    case 'R':
                    case 'r':
                        flags |= MessageFlags.Read;
                        break;
                    case 'S':
                    case 's':
                        flags |= MessageFlags.Seen;
                        break;
                    case 'T':
                    case 't':
                        flags |= MessageFlags.Trashed;
                        break;
                    case 'F':
                    case 'f':
                        flags |= MessageFlags.Flagged;
                        break;
                    default:
                        stop = true;
                        break;
                }
            }
            return flags;
        }
    }
}
