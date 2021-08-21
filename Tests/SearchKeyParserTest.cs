using Meel.Parsing;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Meel.Tests
{
    [TestFixture]
    public class SearchKeyParserTest
    {
        private static readonly ImapMessage[] messages = new[]
        {
            new ImapMessage(null, 1, MessageFlags.None, 256),
            new ImapMessage(null, 2, MessageFlags.Seen | MessageFlags.Recent, 1024),
            new ImapMessage(null, 3, MessageFlags.Seen | MessageFlags.Draft, 256),
            new ImapMessage(null, 4, MessageFlags.Recent, 16)
        };
        
        [Test]
        public void TestAll()
        {
            // Arrange
            var query = "All";
            var expected = new uint[] { 1, 2, 3, 4 };
            // Act
            var actual = GetMatchingMessages(query);
            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestNotAll()
        {
            // Arrange
            var query = "NOT All";
            var expected = new uint[0];
            // Act
            var actual = GetMatchingMessages(query);
            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestDraft()
        {
            // Arrange
            var query = "DRAFT";
            var expected = new uint[] { 3 };
            // Act
            var actual = GetMatchingMessages(query);
            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestUnDraft()
        {
            // Arrange
            var query = "UNDRAFT";
            var expected = new uint[] { 1, 2, 4 };
            // Act
            var actual = GetMatchingMessages(query);
            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestSeen()
        {
            // Arrange
            var query = "SEEN";
            var expected = new uint[] { 2, 3 };
            // Act
            var actual = GetMatchingMessages(query);
            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestNew()
        {
            // Arrange
            var query = "NEW";
            var expected = new uint[] { 4 };
            // Act
            var actual = GetMatchingMessages(query);
            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestOld()
        {
            // Arrange
            var query = "OLD";
            var expected = new uint[] { 1, 3 };
            // Act
            var actual = GetMatchingMessages(query);
            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestRecent()
        {
            // Arrange
            var query = "RECENT";
            var expected = new uint[] { 2, 4 };
            // Act
            var actual = GetMatchingMessages(query);
            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestSmaller()
        {
            // Arrange
            var query = "SMALLER 256";
            var expected = new uint[] { 4 };
            // Act
            var actual = GetMatchingMessages(query);
            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestLarger()
        {
            // Arrange
            var query = "LARGER 256";
            var expected = new uint[] { 2 };
            // Act
            var actual = GetMatchingMessages(query);
            // Assert
            CollectionAssert.AreEquivalent(expected, actual);
        }

        private List<uint> GetMatchingMessages(string query)
        {
            var numMessages = (uint)messages.Length;
            var key = SearchKeyParser.Parse(query.AsAsciiSpan(), numMessages);
            return messages
                .Where(messages => key.Matches(messages, numMessages))
                .Select(r => r.Uid)
                .ToList();
        }
    }
}
