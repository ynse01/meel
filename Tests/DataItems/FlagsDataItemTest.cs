using Meel.DataItems;
using Meel.Responses;
using NUnit.Framework;

namespace Meel.Tests.DataItems
{
    [TestFixture]
    public class FlagsDataItemTest
    {
        [Test]
        public void ShouldNotPrintAnyFlags()
        {
            // Arrange
            var item = new FlagsDataItem();
            var expected = "FLAGS ()";
            var response = new ImapResponse();
            var message = new ImapMessage(null, 1, MessageFlags.None, 0);
            response.Allocate(1000);
            // Act
            item.PrintContent(ref response, message);
            // Assert
            var txt = response.ToString();
            StringAssert.AreEqualIgnoringCase(expected, txt);
        }

        [Test]
        public void ShouldPrintSeenFlags()
        {
            // Arrange
            var item = new FlagsDataItem();
            var expected = "FLAGS (\\Seen)";
            var response = new ImapResponse();
            var message = new ImapMessage(null, 1, MessageFlags.Seen, 0);
            response.Allocate(1000);
            // Act
            item.PrintContent(ref response, message);
            // Assert
            var txt = response.ToString();
            StringAssert.AreEqualIgnoringCase(expected, txt);
        }

        [Test]
        public void ShouldPrintAnsweredFlags()
        {
            // Arrange
            var item = new FlagsDataItem();
            var expected = "FLAGS (\\Answered)";
            var response = new ImapResponse();
            var message = new ImapMessage(null, 1, MessageFlags.Answered, 0);
            response.Allocate(1000);
            // Act
            item.PrintContent(ref response, message);
            // Assert
            var txt = response.ToString();
            StringAssert.AreEqualIgnoringCase(expected, txt);
        }

        [Test]
        public void ShouldPrintDeletedFlags()
        {
            // Arrange
            var item = new FlagsDataItem();
            var expected = "FLAGS (\\Deleted)";
            var response = new ImapResponse();
            var message = new ImapMessage(null, 1, MessageFlags.Deleted, 0);
            response.Allocate(1000);
            // Act
            item.PrintContent(ref response, message);
            // Assert
            var txt = response.ToString();
            StringAssert.AreEqualIgnoringCase(expected, txt);
        }

        [Test]
        public void ShouldPrintDraftFlags()
        {
            // Arrange
            var item = new FlagsDataItem();
            var expected = "FLAGS (\\Draft)";
            var response = new ImapResponse();
            var message = new ImapMessage(null, 1, MessageFlags.Draft, 0);
            response.Allocate(1000);
            // Act
            item.PrintContent(ref response, message);
            // Assert
            var txt = response.ToString();
            StringAssert.AreEqualIgnoringCase(expected, txt);
        }

        [Test]
        public void ShouldPrintFlaggedFlags()
        {
            // Arrange
            var item = new FlagsDataItem();
            var expected = "FLAGS (\\Flagged)";
            var response = new ImapResponse();
            var message = new ImapMessage(null, 1, MessageFlags.Flagged, 0);
            response.Allocate(1000);
            // Act
            item.PrintContent(ref response, message);
            // Assert
            var txt = response.ToString();
            StringAssert.AreEqualIgnoringCase(expected, txt);
        }

        [Test]
        public void ShouldPrintRecentFlags()
        {
            // Arrange
            var item = new FlagsDataItem();
            var expected = "FLAGS (\\Recent)";
            var response = new ImapResponse();
            var message = new ImapMessage(null, 1, MessageFlags.Recent, 0);
            response.Allocate(1000);
            // Act
            item.PrintContent(ref response, message);
            // Assert
            var txt = response.ToString();
            StringAssert.AreEqualIgnoringCase(expected, txt);
        }
    }
}
