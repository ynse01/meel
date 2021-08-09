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
    public class ListCommandTest
    {
        [Test]
        public void ShouldListAllBoxes()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "Existing";
            var unsub = "Unsubscribed";
            station.CreateMailbox(user, box);
            station.SetSubscription(user, box, true);
            station.CreateMailbox(user, unsub);
            station.SetSubscription(user, unsub, false);
            var command = new ListCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Authenticated;
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = "/ %".AsAsciiSpan();
            // Act
            command.Execute(context, requestId, options, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.DoesNotContain("BAD", txt);
            StringAssert.Contains("OK", txt);
            StringAssert.Contains(box, txt);
            StringAssert.Contains(unsub, txt);
        }

        [Test]
        public void ShouldReturnFlags()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var boxName = "Existing";
            var otherName = "Another";
            station.CreateMailbox(user, boxName);
            var box = (InMemoryMailbox)station.SelectMailbox(user, boxName);
            box.SetNoSelect(true);
            station.CreateMailbox(user, otherName);
            var command = new ListCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Authenticated;
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = "/ %".AsAsciiSpan();
            // Act
            command.Execute(context, requestId, options, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.DoesNotContain("BAD", txt);
            StringAssert.Contains("OK", txt);
            var lines = txt.Split("\r\n");
            StringAssert.Contains(boxName, lines[0]);
            StringAssert.Contains(otherName, lines[1]);
            StringAssert.Contains("(/NoSelect)", lines[0]);
        }

        [Test]
        public void ShouldNotListBeforeLogin()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "Existing";
            station.CreateMailbox(user, box);
            var command = new ListCommand(station);
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
        public void ShouldNotListAfterLogout()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "Existing";
            station.CreateMailbox(user, box);
            var command = new ListCommand(station);
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
            var command = new ListCommand(station);
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
