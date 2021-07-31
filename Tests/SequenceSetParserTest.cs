using Meel.Parsing;
using NUnit.Framework;

namespace Meel.Tests
{
    [TestFixture]
    public class SequenceSetParserTest
    {
        [Test]
        public void TestFirstExample()
        {
            // Example: a message sequence number set of
            // 2,4:7,9,12:* for a mailbox with 15 messages is
            // equivalent to 2,4,5,6,7,9,12,13,14,15
            
            // Arrange
            var query = "2,4:7,9,12:*";
            var expected = new uint[] { 2, 4, 5, 6, 7, 9, 12, 13, 14, 15 };
            var numMessages = 15u;
            // Act
            var actual = SequenceSetParser.Parse(query.AsAsciiSpan(), numMessages);
            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestSecondExample()
        {
            // Example: a message sequence number set of *:4,5:7
            // for a mailbox with 10 messages is equivalent to
            // 10,9,8,7,6,5,4,5,6,7 and MAY be reordered and
            // overlap coalesced to be 4,5,6,7,8,9,10.

            // Arrange
            var query = "*:4,5:7";
            var expected = new uint[] { 4, 5, 6, 7, 8, 9, 10 };
            var numMessages = 10u;
            // Act
            var actual = SequenceSetParser.Parse(query.AsAsciiSpan(), numMessages);
            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
