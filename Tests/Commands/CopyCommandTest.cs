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
    public class CopyCommandTest
    {
        [Test]
        public void ShouldCopyMessages()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var sourceName = "Source";
            var destName = "Destination";
            var sequence = "1:*";
            station.CreateMailbox(user, sourceName);
            var box = station.SelectMailbox(user, sourceName);
            var message = new ImapMessage(null, 1u, MessageFlags.None, 0L);
            station.AppendToMailbox(box, message);
            station.CreateMailbox(user, destName);
            var command = new CopyCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.Username = user;
            context.SetSelectedMailbox(box);
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = $"{sequence} {destName}".AsAsciiSpan();
            // Act
            command.Execute(context, requestId, options, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.DoesNotContain("BAD", txt);
            StringAssert.DoesNotContain("NO", txt);
            StringAssert.Contains("OK", txt);
        }

        [Test]
        public void ShouldFailToCopyToNonExistingBox()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var sourceName = "Source";
            var destName = "NonExisting";
            var sequence = "1:*";
            station.CreateMailbox(user, sourceName);
            var box = station.SelectMailbox(user, sourceName);
            var command = new CopyCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.Username = user;
            context.SetSelectedMailbox(box);
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = $"{sequence} {destName}".AsAsciiSpan();
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
        public void ShouldFailToCopyNonExistingMessages()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var sourceName = "Source";
            var destName = "Destination";
            var sequence = "100";
            station.CreateMailbox(user, sourceName);
            var box = station.SelectMailbox(user, sourceName);
            station.CreateMailbox(user, destName);
            var command = new CopyCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.Username = user;
            context.SetSelectedMailbox(box);
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = $"{sequence} {destName}".AsAsciiSpan();
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
        public void ShouldNotCopyBeforeLogin()
        {
            // Arrange
            var station = new InMemoryStation();
            var sequence = "1:*";
            var user = "Piet";
            var box = "Existing";
            station.CreateMailbox(user, box);
            var command = new CopyCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = $"{sequence} {box}".AsAsciiSpan();
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
        public void ShouldNotCopyAfterLogout()
        {
            // Arrange
            var station = new InMemoryStation();
            var sequence = "1:*";
            var user = "Piet";
            var box = "Existing";
            station.CreateMailbox(user, box);
            var command = new CopyCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Logout;
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = $"{sequence} {box}".AsAsciiSpan();
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
        public void ShouldReturnBadWithSingleArgument()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "Existing";
            station.CreateMailbox(user, box);
            var command = new CopyCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Selected;
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
            var command = new CopyCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Selected;
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
