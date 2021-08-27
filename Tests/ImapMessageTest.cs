using MimeKit;
using NUnit.Framework;
using System.Linq;

namespace Meel.Tests
{
    [TestFixture]
    public class ImapMessageTest
    {
        [Test]
        public void Test()
        {
            // Arrange
            var resource = TestHelper.LoadEmbeddedResource("Meel.Tests.DataItems.SimpleMessage.txt");
            // Act
            var message = new ImapMessage(MimeMessage.Load(resource), 42, MessageFlags.None, 0);
            // Assert
            StringAssert.AreEqualIgnoringCase("\"Meel\" <ynse@meel.io>", message.Message.From.ToString());
            StringAssert.AreEqualIgnoringCase("ynse@meel.io", message.Message.To.ToString());
            Assert.AreEqual(1, message.Message.MimeVersion.Major);
            Assert.AreEqual(0, message.Message.MimeVersion.Minor);
            Assert.AreEqual("3f2a4a19-7c2f-3196-e847-30a1b3882e1b@meel.io", message.Message.MessageId);
            Assert.AreEqual(1, message.Message.BodyParts.Count());
            var bodyPart = message.Message.BodyParts.First();
            Assert.AreEqual("text/plain", bodyPart.ContentType.MimeType);
        }
    }
}
