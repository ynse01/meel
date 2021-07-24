using Meel.Stations;
using NUnit.Framework;

namespace Meel.Tests
{
    [TestFixture]
    public class InMemoryStationTest
    {
        [Test]
        [TestCase("INBOX")]
        [TestCase("InBox")]
        [TestCase("inbox")]
        public void SelectShouldMatchCaseInsensitive(string name)
        {
            // Arrange
            var station = new InMemoryStation();
            station.CreateMailbox("", "INBOX");
            // Act
            var actual = station.SelectMailbox("", name);
            // Assert
            Assert.IsNotNull(actual);
        }

        [Test]
        public void ShouldNotSelectMailboxThatDoesNotExists()
        {
            // Arrange
            var station = new InMemoryStation();
            station.CreateMailbox("Piet", "INBOX");
            // Act
            var actual = station.SelectMailbox("Piet", "OutBox");
            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void ShouldNotSelectForUserThatDoesNotExists()
        {
            // Arrange
            var station = new InMemoryStation();
            station.CreateMailbox("Piet", "INBOX");
            // Act
            var actual = station.SelectMailbox("Klaas", "INBOX");
            // Assert
            Assert.IsNull(actual);
        }

        [Test]
        public void ShouldNotSelectMailboxFromOtherUser()
        {
            // Arrange
            var station = new InMemoryStation();
            station.CreateMailbox("Piet", "INBOX");
            station.CreateMailbox("Klaas", "INBOX");
            var klaas = station.SelectMailbox("Klaas", "INBOX");
            station.AppendToMailbox(klaas, new ImapMessage(null, null, MessageFlags.None, 0));
            // Act
            var actual = station.SelectMailbox("Klaas", "INBOX");
            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(1, actual.NumberOfMessages);
        }

        [Test]
        public void ShouldListAllMailboxForUser()
        {
            // Arrange
            var station = new InMemoryStation();
            station.CreateMailbox("Piet", "INBOX");
            station.CreateMailbox("Piet", "OutBox");
            station.CreateMailbox("Piet", "Sent");
            // Act
            var actual = station.ListMailboxes("Piet", false);
            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(3, actual.Count);
        }

        [Test]
        public void ShouldNotListMailboxForOtherUser()
        {
            // Arrange
            var station = new InMemoryStation();
            station.CreateMailbox("Piet", "INBOX");
            // Act
            var actual = station.ListMailboxes("Klaas", false);
            // Assert
            Assert.IsNotNull(actual);
            Assert.AreEqual(0, actual.Count);
        }

        [Test]
        public void ShouldNotAppendToReadOnlyMailbox()
        {
            // Arrange
            var station = new InMemoryStation();
            station.CreateMailbox(null, "ReadOnly");
            var readOnly = station.SelectMailbox(null, "ReadOnly");
            ((InMemoryMailbox)readOnly).SetReadonly();
            // Act
            var actual = station.AppendToMailbox(readOnly, null);
            // Assert
            Assert.IsFalse(actual);
        }
    }
}