using Meel.Commands;
using Meel.Parsing;
using Meel.Responses;
using Meel.Stations;
using NUnit.Framework;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Tests.Commands
{
    [TestFixture]
    public class ExpungeCommandTest
    {
        [Test]
        public void ShouldExpungeExistingBox()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var boxName = "Existing";
            station.CreateMailbox(user, boxName);
            var box = station.SelectMailbox(user, boxName);
            var command = new ExpungeCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Selected;
            context.Username = user;
            context.SetSelectedMailbox(box);
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = ReadOnlySpan<byte>.Empty;
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
        public void ShouldNotExpungeNonExistingBox()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var boxName = "Existing";
            station.CreateMailbox(user, boxName);
            var command = new ExpungeCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Selected;
            context.Username = user;
            context.SetSelectedMailbox(null);
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = ReadOnlySpan<byte>.Empty;
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
        public void ShouldNotExpungeBeforeLogin()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "Existing";
            station.CreateMailbox(user, box);
            var command = new ExpungeCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = ReadOnlySpan<byte>.Empty;
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
        public void ShouldNotExpungeWhenNotSelected()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "Existing";
            station.CreateMailbox(user, box);
            var command = new ExpungeCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Authenticated;
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = ReadOnlySpan<byte>.Empty;
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
        public void ShouldNotExpungeAfterLogout()
        {
            // Arrange
            var station = new InMemoryStation();
            var user = "Piet";
            var box = "Existing";
            station.CreateMailbox(user, box);
            var command = new ExpungeCommand(station);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            context.State = SessionState.Logout;
            context.Username = user;
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            var options = ReadOnlySpan<byte>.Empty;
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
