using Meel.Commands;
using Meel.Responses;
using NUnit.Framework;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Tests.Commands
{
    [TestFixture]
    public class BadCommandTest
    {
        [Test]
        public void ShouldReturnBad()
        {
            // Arrange
            var command = new BadCommand(null);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            // Act
            command.Execute(context, requestId, ReadOnlySpan<byte>.Empty, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.DoesNotContain("OK", txt);
            StringAssert.DoesNotContain("NO", txt);
            StringAssert.Contains("BAD", txt);
        }
    }
}
