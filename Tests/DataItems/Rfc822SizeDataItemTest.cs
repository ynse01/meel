using Meel.DataItems;
using Meel.Responses;
using NUnit.Framework;

namespace Meel.Tests.DataItems
{
    [TestFixture]
    public class Rfc822SizeDataItemTest
    {
        [Test]
        public void ShouldPrintSize()
        {
            // Arrange
            var item = new Rfc822SizeDataItem();
            var expected = "RFC822.SIZE 42";
            var response = new ImapResponse();
            var message = new ImapMessage(null, 1, MessageFlags.None, 42);
            response.Allocate(1000);
            // Act
            item.PrintContent(ref response, message);
            // Assert
            var txt = response.ToString();
            StringAssert.AreEqualIgnoringCase(expected, txt);
        }
    }
}
