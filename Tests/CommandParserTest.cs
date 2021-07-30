using Meel.Commands;
using Meel.Parsing;
using NUnit.Framework;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Tests
{
    [TestFixture]
    public class CommandParserTest
    {
        [Test]
        [TestCase("APPEND", ImapCommands.Append)]
        [TestCase("AUTHENTICATE", ImapCommands.Authenticate)]
        [TestCase("BAD", ImapCommands.Bad)]
        [TestCase("CAPABILITY", ImapCommands.Capability)]
        [TestCase("CHECK", ImapCommands.Check)]
        [TestCase("CLOSE", ImapCommands.Close)]
        [TestCase("COPY", ImapCommands.Copy)]
        [TestCase("CREATE", ImapCommands.Create)]
        [TestCase("DELETE", ImapCommands.Delete)]
        [TestCase("EXAMINE", ImapCommands.Examine)]
        [TestCase("EXPUNGE", ImapCommands.Expunge)]
        [TestCase("FETCH", ImapCommands.Fetch)]
        [TestCase("LIST", ImapCommands.List)]
        [TestCase("LOGIN", ImapCommands.Login)]
        [TestCase("LOGOUT", ImapCommands.Logout)]
        [TestCase("LSUB", ImapCommands.LSub)]
        [TestCase("NOOP", ImapCommands.Noop)]
        [TestCase("RENAME", ImapCommands.Rename)]
        [TestCase("SEARCH", ImapCommands.Search)]
        [TestCase("SELECT", ImapCommands.Select)]
        [TestCase("STARTTLS", ImapCommands.StartTls)]
        [TestCase("STATUS", ImapCommands.Status)]
        [TestCase("STORE", ImapCommands.Store)]
        [TestCase("SUBSCRIBE", ImapCommands.Subscribe)]
        [TestCase("UID", ImapCommands.Uid)]
        [TestCase("UNSUBSCRIBE", ImapCommands.Unsubscribe)]
        public void TestCommand(string input, ImapCommands command)
        {
            // Arrange
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(input));
            // Act
            var actual = CommandParser.Parse(buffer, out ReadOnlySpan<byte> options);
            // Assert
            Assert.AreEqual(command, actual);
        }

        [Test]
        [TestCase("UID")]
        [TestCase("uid")]
        [TestCase("Uid")]
        [TestCase("UID SELECT")]
        [TestCase("uid SELECT")]
        [TestCase("Uid INVALID")]
        public void TestCasing(string input)
        {
            // Arrange
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(input));
            // Act
            var actual = CommandParser.Parse(buffer, out ReadOnlySpan<byte> options);
            // Assert
            Assert.AreEqual(ImapCommands.Uid, actual);
        }

        [Test]
        [TestCase("NOTEXISTS")]
        [TestCase("INVALID")]
        [TestCase("BAD")]
        [TestCase("")]
        [TestCase("[SELECT]")]
        [TestCase("(SELECT)")]
        [TestCase("{SELECT}")]
        public void TestInvalid(string input)
        {
            // Arrange
            var buffer = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes(input));
            // Act
            var actual = CommandParser.Parse(buffer, out ReadOnlySpan<byte> options);
            // Assert
            Assert.AreEqual(ImapCommands.Bad, actual);
        }
    }
}
