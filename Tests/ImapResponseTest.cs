using Meel.Parsing;
using Meel.Responses;
using NUnit.Framework;
using System.IO;
using System.IO.Pipelines;
using System.Text;

namespace Meel.Tests
{
    [TestFixture]
    public class ImapResponseTest
    {
        [Test]
        public void ShouldReturnEntireInput()
        {
            // Arrange
            var response = new ImapResponse();
            response.Allocate(1000);
            response.Append("Just a string");
            response.AppendSpace();
            response.AppendLine("and a bow.".AsAsciiSpan());
            var expected = "Just a string and a bow.\r\n";
            // Act
            var actual = response.ToString();
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ShouldReturnEntireInputToPipe()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                var writer = PipeWriter.Create(stream);
                var response = new ImapResponse(writer);
                response.Allocate(1000);
                response.Append("Just a string");
                response.AppendSpace();
                response.AppendLine("and a bow.".AsAsciiSpan());
                var expected = "Just a string and a bow.\r\n";
                // Act
                var actual = response.ToString();
                // Assert
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void ShouldReturnEntireStreamContent()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                var writer = PipeWriter.Create(stream);
                var response = new ImapResponse(writer);
                var input = response.GetStream();
                var expected = "Just a string and a bow.\r\n";
                // Act
                input.Write(Encoding.ASCII.GetBytes(expected));
                stream.Flush();
                // Assert
                var actual = Encoding.ASCII.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
