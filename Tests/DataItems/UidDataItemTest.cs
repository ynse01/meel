using Meel.DataItems;
using Meel.Responses;
using NUnit.Framework;

namespace Meel.Tests.DataItems
{
    [TestFixture]
    public class UidDataItemTest
    {
        [Test]
        public void ShouldPrintUid()
        {
            // Arrange
            var item = new UidDataItem();
            var expected = "UID 42";
            var response = new ImapResponse();
            var message = new ImapMessage(null, 42, MessageFlags.None, 0);
            response.Allocate(1000);
            // Act
            item.PrintContent(ref response, message);
            // Assert
            var txt = response.ToString();
            StringAssert.AreEqualIgnoringCase(expected, txt);
        }
    }
}
