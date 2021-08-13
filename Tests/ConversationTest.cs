using Meel.Stations;
using NUnit.Framework;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Meel.Tests
{
    [TestFixture]
    public class ConversationTest
    {
        [Test]
        public async Task ShouldReturnGreeting()
        {
            // Arrange
            var input = "";
            var expected = "* OK Meel server ready for action\r\n";
            // Act
            var actual = await HaveConversation(input);
            // Assert
            StringAssert.AreEqualIgnoringCase(expected, actual);
        }

        private async Task<string> HaveConversation(string input)
        {
            var station = new InMemoryStation();
            var pipe = new ServerPipe(station);
            string response;
            using (var output = new MemoryStream())
            {
                using (var inputStream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(inputStream))
                    {
                        writer.Write(input);
                        writer.Flush();
                        await pipe.ProcessAsync(inputStream, output);
                        response = Encoding.ASCII.GetString(output.GetBuffer(), 0, (int)output.Length);
                    }
                }
            }
            return response;
        }
    }
}
