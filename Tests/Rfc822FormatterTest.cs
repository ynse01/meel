using System;
using NUnit.Framework;
using Meel.Parsing;
using MimeKit;

namespace Meel.Tests
{
    [TestFixture]
    public class Rfc822FormatterTest
    {
        [Test]
        public void TestDateFormatting()
        {
            // Arrange
            var date = new DateTimeOffset(2006, 1, 2, 15, 4, 5, TimeSpan.FromMinutes(-7 * 60));
            var buffer = new byte[100];
            var expected = " 2-Jan-2006 15:04:05 -0700";
            // Act
            var result = Rfc822Formatter.TryFormat(date, buffer, out int bytesWritten);
            // Assert
            var actual = ((ReadOnlySpan<byte>)buffer).AsString();
            Assert.IsTrue(result);
            StringAssert.AreEqualIgnoringCase(expected, actual);
            Assert.AreEqual(26, bytesWritten);
        }

        [Test]
        public void TestAddressFormatting()
        {
            // Arrange
            var addresses = new InternetAddressList(new[] { new MailboxAddress("Piet", "piet@puk.nl") });
            var buffer = new byte[100];
            var expected = "((\"Piet\" NIL \"piet\" \"puk.nl\"))";
            // Act
            var result = Rfc822Formatter.TryFormat(addresses, buffer, out int bytesWritten);
            // Assert
            var actual = ((ReadOnlySpan<byte>)buffer).AsString();
            Assert.IsTrue(result);
            StringAssert.AreEqualIgnoringCase(expected, actual);
            Assert.AreEqual(30, bytesWritten);
        }
    }
}
