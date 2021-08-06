using Meel.Commands;
using Meel.Responses;
using NUnit.Framework;
using System;
using System.Buffers;
using System.Text;

namespace Meel.Tests.Commands
{
    [TestFixture]
    public class CapabilityCommandTest
    {
        [Test]
        public void Test()
        {
            // Arrange
            var command = new CapabilityCommand(null);
            var response = new ImapResponse();
            var context = new ConnectionContext(42);
            var requestId = new ReadOnlySequence<byte>(Encoding.ASCII.GetBytes("123"));
            // Act
            command.Execute(context, requestId, ReadOnlySpan<byte>.Empty, ref response);
            // Assert
            var txt = response.ToString();
            Assert.IsNotNull(txt);
            StringAssert.DoesNotContain("BAD", txt);
            StringAssert.Contains("OK", txt);
            StringAssert.EndsWith("123 OK CAPABILITY completed\r\n", txt);
        }
    }
}
