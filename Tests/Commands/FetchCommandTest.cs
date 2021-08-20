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
    public class FetchCommandTest
    {
        [Test]
        public void ShouldFetch()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var boxName = "Existing";
            var query = "1 FLAGS";
            var expected = "* 1 FETCH (FLAGS (\\Draft))";
            station.CreateMailbox(user, boxName);
            var box = station.SelectMailbox(user, boxName);
            var message = new ImapMessage(null, 1, MessageFlags.Draft, 0);
            station.AppendToMailbox(box, message);
            var command = new FetchCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Selected;
            context.Username = user;
            context.SetSelectedMailbox(box);
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = query.AsAsciiSpan();
            // Act
            command.Execute(context, requestId, options, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.DoesNotContain("BAD", txt);
            StringAssert.DoesNotContain("NO", txt);
            StringAssert.Contains("OK", txt);
            StringAssert.Contains(expected, txt);
        }

        [Test]
        public void ShouldNotFetchFromNonExistingMessage()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var boxName = "Existing";
            var query = "2 FLAGS";
            station.CreateMailbox(user, boxName);
            var box = station.SelectMailbox(user, boxName);
            var message = new ImapMessage(null, 1, MessageFlags.Draft, 0);
            station.AppendToMailbox(box, message);
            var command = new FetchCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Selected;
            context.Username = user;
            context.SetSelectedMailbox(box);
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = query.AsAsciiSpan();
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
        public void ShouldNotFetchFromNonExistingBox()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var query = "1 FLAGS";
            var command = new FetchCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.SetSelectedMailbox(null);
            context.State = SessionState.Selected;
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = query.AsAsciiSpan();
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
        public void ShouldNotFetchBeforeLogin()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var query = "1 FLAGS";
            var command = new FetchCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = query.AsAsciiSpan();
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
        public void ShouldNotFetchAfterLogout()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var query = "1 FLAGS";
            var command = new SearchCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Logout;
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = query.AsAsciiSpan();
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
            var command = new FetchCommand(station);
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
