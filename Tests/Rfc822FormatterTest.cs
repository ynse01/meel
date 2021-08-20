using System;
using NUnit.Framework;
using Meel.Parsing;
using MimeKit;
using Meel.Responses;

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
            var response = new ImapResponse();
            response.Allocate(100);
            var expected = " 2-Jan-2006 15:04:05 -0700";
            // Act
            var result = Rfc822Formatter.TryFormat(date, ref response);
            // Assert
            var actual = response.ToString();
            Assert.IsTrue(result);
            StringAssert.AreEqualIgnoringCase(expected, actual);
            Assert.AreEqual(26, response.Length);
        }

        [Test]
        public void TestAddressFormatting()
        {
            // Arrange
            var addresses = new InternetAddressList(new[] { new MailboxAddress("Piet", "piet@puk.nl") });
            var response = new ImapResponse();
            response.Allocate(100);
            var expected = "((\"Piet\" NIL \"piet\" \"puk.nl\"))";
            // Act
            var result = Rfc822Formatter.TryFormat(addresses, ref response);
            // Assert
            var actual = response.ToString();
            Assert.IsTrue(result);
            StringAssert.AreEqualIgnoringCase(expected, actual);
            Assert.AreEqual(30, response.Length);
        }
    }
}
