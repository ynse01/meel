using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Meel.Responses
{
    public class ImapResponse
    {
        private const string EndOfLine = "\r\n";

        private List<string> lines = new List<string>();
        private int index = 0;

        public int ExpectLiteralOfSize { get; set; }

        public void Write(params string[] messages)
        {
            var line = string.Join(' ', messages);
            SafeWrite(line);
        }

        public void WriteLine(params string[] messages)
        {
            var line = string.Join(' ', messages) + EndOfLine;
            SafeWrite(line);
            index++;
        }

        public void SendTo(Stream stream)
        {
            foreach(var line in lines)
            {
                var bytes = Encoding.ASCII.GetBytes(line);
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public override string ToString()
        {
            return string.Concat(lines);
        }

        private void SafeWrite(string message)
        {
            if (lines.Count == index)
            {
                lines.Add(message);
            } else
            {
                lines[index] += message;
            }
        }
    }
}
