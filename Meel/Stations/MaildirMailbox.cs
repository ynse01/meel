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

        public override ImapMessage GetMessage(uint sequenceId)
        {
            var filename = files[(int)sequenceId];
            using (var stream = new FileStream(filename, FileMode.Open))
            {
                var message = MimeMessage.Load(stream);
                var flags = ParseFlags(filename);
                var uid = GetUid(filename);
                return new ImapMessage(message, uid, flags, stream.Length);
            }
        }

        public override uint GetSequenceNumber(ImapMessage message)
        {
            // TODO: Implement
            return 0;
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

        public List<uint> Expunge()
        {
            var deleted = new List<uint>();
            // TODO: Implement
            return deleted;
        }

        public List<uint> SearchMessagesBySequence(ISearchKey searchKey)
        {
            var found = new List<uint>();
            var needsFullLoad = NeedsFullLoad(searchKey);
            for (var i = 0; i < files.Count; i++)
            {
                var message = OpenMessage(files[i], needsFullLoad);
                if (searchKey.Matches(message, (uint)(i + 1)))
                {
                    found.Add((uint)(i + 1));
                }
            }
            return found;
        }

        public List<uint> SearchMessagesByUid(ISearchKey searchKey)
        {
            var found = new List<uint>();
            var needsFullLoad = NeedsFullLoad(searchKey);
            for (int i = 0; i < files.Count; i++)
            {
                var message = OpenMessage(files[i], needsFullLoad);
                if (searchKey.Matches(message, (uint)(i + 1)))
                {
                    found.Add(message.Uid);
                }
            }
            return found;
        }

        public uint Sequence2Uid(uint sequenceId)
        {
            return GetUid(files[(int)(sequenceId - 1)]);
        }

        public uint GetUid(string filename)
        {
            return uint.Parse(filename.Split('.', 1)[0]);
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
                        flags |= MessageFlags.Answered;
                        break;
                    case 'S':
                    case 's':
                        flags |= MessageFlags.Seen;
                        break;
                    case 'T':
                    case 't':
                        flags |= MessageFlags.Deleted;
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

        private bool NeedsFullLoad(ISearchKey searchKey)
        {
            var searchDepth = searchKey.GetSearchDepth();
            var needsFullLoad = searchDepth.HasFlag(SearchDepth.Header) | searchDepth.HasFlag(SearchDepth.Body);
            return needsFullLoad;
        }

        private ImapMessage OpenMessage(string filename, bool loadFully)
        {
            MimeMessage mime = null;
            var fullPath = Path.Combine(path, filename);
            if (loadFully)
            {
                mime = MimeMessage.Load(fullPath);
            }
            var uid = GetUid(filename);
            var flags = ParseFlags(filename);
            var size = new FileInfo(fullPath).Length;
            var message = new ImapMessage(mime, uid, flags, size);
            return message;
        }
    }
}
