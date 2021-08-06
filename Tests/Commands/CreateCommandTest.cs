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
    public class CreateCommandTest
    {
        [Test]
        public void ShouldCreateBox()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "Existing";
            var command = new CreateCommand(station);
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
            StringAssert.Contains("OK", txt);
            Assert.IsNotNull(station.SelectMailbox(user, box));
        }

        [Test]
        public void ShouldNotCreateExistingBox()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "Existing";
            station.CreateMailbox(user, box);
            var command = new CreateCommand(station);
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
        public void ShouldNotCreateBeforeLogin()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "Existing";
            var command = new CreateCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = box.AsAsciiSpan();
            // Act
            command.Execute(context, requestId, options, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.DoesNotContain("OK", txt);
            StringAssert.Contains("BAD", txt);
        }
    }
}
