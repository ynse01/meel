using Meel.Parsing;
using Meel.Stations;
using NUnit.Framework;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Meel.Tests
{
    [TestFixture]
    public class ConversationTest
    {
        [Test]
        public void ShouldReturnGreeting()
        {
            // Arrange
            var station = new InMemoryStation();
            var input = new[] { "" };
            var expected = new List<string> { "* OK Meel server ready for action" };
            // Act
            var actual = HaveConversation(station, input);
            // Assert
            CollectionAssert.AreEqual(expected, actual, StringComparer.OrdinalIgnoreCase);
        }

        [Test]
        public void ShouldLogin()
        {
            // Arrange
            var station = new InMemoryStation();
            var input = new[] { "a001 login piet secret" };
            var expected = new List<string> { 
                "* OK Meel server ready for action",
                "a001 OK LOGIN completed"
            };
            // Act
            var actual = HaveConversation(station, input);
            // Assert
            CollectionAssert.AreEqual(expected, actual, StringComparer.OrdinalIgnoreCase);
        }

        [Test]
        [Explicit]
        public void ShouldResembleExampleFromRfc3501()
        {
            // Arrange
            var station = new InMemoryStation();
            var input = new[] { 
                "a001 login mrc secret",
                "a002 select inbox",
                "a003 fetch 12 full",
                "a004 fetch 12 body[header]",
                @"a005 store 12 +flags \deleted",
                "a006 logout"
            };
            var expected = new List<string> { 
                "* OK Meel server ready for action",
                "a001 OK LOGIN completed",
                "* 18 EXISTS",
                @"* FLAGS (\Answered \Flagged \Deleted \Seen \Draft)",
                "* 2 RECENT",
                "* OK [UNSEEN 17] Message 17 is the first unseen message",
                "* OK [UIDVALIDITY 3857529045] UIDs valid",
                "a002 OK [READ-WRITE] SELECT completed",
                "* 12 FETCH (FLAGS (\\Seen) INTERNALDATE \"17-Jul-1996 02:44:25 - 0700\" RFC822.SIZE 4286 ENVELOPE(\"Wed, 17 Jul 1996 02:23:25 -0700 (PDT)\" \"IMAP4rev1 WG mtg summary and minutes\" ((\"Terry Gray\" NIL \"gray\" \"cac.washington.edu\")) ((\"Terry Gray\" NIL \"gray\" \"cac.washington.edu\")) ((\"Terry Gray\" NIL \"gray\" \"cac.washington.edu\")) ((NIL NIL \"imap\" \"cac.washington.edu\")) ((NIL NIL \"minutes\" \"CNRI.Reston.VA.US\") (\"John Klensin\" NIL \"KLENSIN\" \"MIT.EDU\")) NIL NIL \"<B27397-0100000@cac.washington.edu>\") BODY(\"TEXT\" \"PLAIN\"(\"CHARSET\" \"US-ASCII\") NIL NIL \"7BIT\" 3028 92))",
                "a003 OK FETCH completed",
                "* 12 FETCH (BODY[HEADER] {342}",
                "Date: Wed, 17 Jul 1996 02:23:25 -0700 (PDT)",
                "From: Terry Gray <gray@cac.washington.edu>",
                "Subject: IMAP4rev1 WG mtg summary and minutes",
                "To: imap@cac.washington.edu",
                "cc: minutes@CNRI.Reston.VA.US, John Klensin <KLENSIN@MIT.EDU>",
                "Message-Id: <B27397-0100000@cac.washington.edu>",
                "MIME-Version: 1.0",
                "Content-Type: TEXT/PLAIN; CHARSET=US-ASCII",
                "",
                ")",
                "a004 OK FETCH completed",
                @"* 12 FETCH (FLAGS (\Seen \Deleted))",
                "a005 OK +FLAGS completed",
                "* BYE IMAP4rev1 server terminating connection",
                "a006 OK LOGOUT completed"
            };
            // Act
            var actual = HaveConversation(station, input);
            // Assert
            CollectionAssert.AreEqual(expected, actual, StringComparer.OrdinalIgnoreCase);
        }

        private List<string> HaveConversation(IMailStation station, IEnumerable<string> input)
        {
            var pipe = new ServerPipe(station);
            List<string> response;
            using (var output = new MemoryStream())
            {
                var length = 0;
                foreach (var request in input)
                {
                    length += request.Length + 2;
                }
                var buffer = new byte[length];
                var offset = 0;
                foreach (var request in input)
                {
                    var temp = Encoding.ASCII.GetBytes(request);
                    Buffer.BlockCopy(temp, 0, buffer, offset, temp.Length);
                    offset += temp.Length;
                    buffer[offset++] = LexiConstants.CarrageReturn;
                    buffer[offset++] = LexiConstants.NewLine;
                }
                var sequence = new ReadOnlySequence<byte>(buffer);
                pipe.ProcessAsync(ref sequence, output);
                response = SplitInLines(output.GetBuffer(), (int)output.Length);
            }
            return response;
        }

        private List<string> SplitInLines(byte[] buffer, int bufferLength)
        {
            var lines = new List<string>();
            var start = 0;
            for(var i = start; i < bufferLength - 1; i++)
            {
                if (buffer[i] == LexiConstants.CarrageReturn && buffer[i + 1] == LexiConstants.NewLine)
                {
                    lines.Add(Encoding.ASCII.GetString(buffer, start, i - start));
                    start = i + 2;
                }
            }
            return lines;
        }
    }
}
