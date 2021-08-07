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
            station.CreateMailbox(user, box);
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
