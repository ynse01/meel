using Meel.DataItems;
using Meel.Responses;
using MimeKit;
using NUnit.Framework;
using System.Text;

namespace Meel.Tests.DataItems
{
    [TestFixture]
    public class BodySectionDataItemTest
    {

        [Test]
        public void Test()
        {
            // Arrange
            var section = new BodySection(new[] { 2 }, BodySubset.Text);
            var item = new BodySectionDataItem(section);
            var expected = "BODY[2.TEXT] (Some simple text\r\n)";
            var response = new ImapResponse();
            var resource = TestHelper.LoadEmbeddedResource("Meel.Tests.DataItems.SimpleMessage.txt");
            var message = new ImapMessage(MimeMessage.Load(resource), 42, MessageFlags.None, 0);
            response.Allocate(1000);
            // Act
            item.PrintContent(ref response, message);
            // Assert
            var txt = response.ToString();
            StringAssert.AreEqualIgnoringCase(expected, txt);
        }
    }
}
