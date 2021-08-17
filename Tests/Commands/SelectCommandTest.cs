using Meel.Commands;
using Meel.Parsing;
using Meel.Responses;
using Meel.Stations;
using NUnit.Framework;
using System.Buffers;
using System.Text;

namespace Meel.Tests.Commands
{
    [TestFixture]
    public class SelectCommandTest
    {
        [Test]
        public void ShouldSelectExistingBox()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "Existing";
            station.CreateMailbox(user, box);
            var command = new SelectCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Authenticated;
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = box.AsAsciiSpan();
            // Act
            command.Execute(context, requestId, options, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.DoesNotContain("BAD", txt);
            StringAssert.DoesNotContain("NO", txt);
            StringAssert.Contains("OK", txt);
            Assert.IsNotNull(station.SelectMailbox(user, box));
        }

        [Test]
        public void ShouldSelectInboxEvenIfNotExistsYet()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "INBOX";
            var command = new SelectCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Authenticated;
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = box.AsAsciiSpan();
            // Act
            command.Execute(context, requestId, options, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.DoesNotContain("BAD", txt);
            StringAssert.DoesNotContain("NO", txt);
            StringAssert.Contains("OK", txt);
            Assert.IsNotNull(station.SelectMailbox(user, box));
        }

        [Test]
        public void ShouldNotSelectNonExistingBox()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "NonExisting";
            var command = new SelectCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Authenticated;
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = box.AsAsciiSpan();
            // Act
            command.Execute(context, requestId, options, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.DoesNotContain("OK", txt);
            StringAssert.DoesNotContain("BAD", txt);
            StringAssert.Contains("NO", txt);
        }

        [Test]
        public void ShouldHaveProperStatistics()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var boxName = "Existing";
            station.CreateMailbox(user, boxName);
            var box = station.SelectMailbox(user, boxName);
            station.AppendToMailbox(box, new ImapMessage(null, 41, MessageFlags.Seen, 0L));
            station.AppendToMailbox(box, new ImapMessage(null, 42, MessageFlags.Seen, 0L));
            station.AppendToMailbox(box, new ImapMessage(null, 43, MessageFlags.Recent, 0L));
            station.AppendToMailbox(box, new ImapMessage(null, 44, MessageFlags.Recent, 0L));
            var command = new SelectCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Authenticated;
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = boxName.AsAsciiSpan();
            // Act
            command.Execute(context, requestId, options, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.Contains("* 4 EXISTS", txt);
            StringAssert.Contains("* 2 RECENT", txt);
            StringAssert.Contains("* OK [UNSEEN 3]", txt);
            StringAssert.Contains("* OK [UIDVALIDITY 3857529045]", txt);
            StringAssert.Contains(@"* FLAGS (\Answered \Flagged \Deleted \Seen \Draft)", txt);
            StringAssert.Contains(@"* OK [PERMANENTFLAGS (\Answered \Flagged \Deleted \Seen \Draft)]", txt);
        }

        [Test]
        public void ShouldNotSelectBeforeLogin()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "Existing";
            var command = new SelectCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = box.AsAsciiSpan();
            // Act
            command.Execute(context, requestId, options, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.DoesNotContain("OK", txt);
            StringAssert.DoesNotContain("NO", txt);
            StringAssert.Contains("BAD", txt);
        }

        [Test]
        public void ShouldNotSelectAfterLogout()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "Existing";
            var command = new SelectCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Logout;
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = box.AsAsciiSpan();
            // Act
            command.Execute(context, requestId, options, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.DoesNotContain("OK", txt);
            StringAssert.DoesNotContain("NO", txt);
            StringAssert.Contains("BAD", txt);
        }

        [Test]
        public void ShouldReturnBadWithoutArgument()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var command = new SelectCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Authenticated;
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = "".AsAsciiSpan();
            // Act
            command.Execute(context, requestId, options, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.DoesNotContain("OK", txt);
            StringAssert.DoesNotContain("NO", txt);
            StringAssert.Contains("BAD", txt);
        }
    }
}
