using Meel.DataItems;
using Meel.Parsing;
using NUnit.Framework;
using System.Collections.Generic;

namespace Meel.Tests
{
    [TestFixture]
    public class DataItemParserTest
    {
        private static readonly ImapMessage complexMessages = new ImapMessage(null, 1, MessageFlags.None, 0);
        
        [Test]
        public void TestAll()
        {
            // Arrange
            var query = "All".AsAsciiSpan();
            var expected = new string[] { "FLAGS", "INTERNALDATE", "RFC822.SIZE", "ENVELOPE" };
            // Act
            var item = DataItemParser.Parse(query);
            // Assert
            var actual = new List<string>();
            GetItemNames(actual, item);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestFast()
        {
            // Arrange
            var query = "Fast".AsAsciiSpan();
            var expected = new string[] { "FLAGS", "INTERNALDATE", "RFC822.SIZE" };
            // Act
            var item = DataItemParser.Parse(query);
            // Assert
            var actual = new List<string>();
            GetItemNames(actual, item);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestFull()
        {
            // Arrange
            var query = "Full".AsAsciiSpan();
            var expected = new string[] { "FLAGS", "INTERNALDATE", "RFC822.SIZE", "ENVELOPE", "BODY" };
            // Act
            var item = DataItemParser.Parse(query);
            // Assert
            var actual = new List<string>();
            GetItemNames(actual, item);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        [TestCase("HEADER")]
        [TestCase("TEXT")]
        [TestCase("HEADER.FIELDS")]
        [TestCase("HEADER.FIELDS.NOT")]
        [TestCase("1.TEXT")]
        [TestCase("2.3.MIME")]
        public void TestBodySections(string section)
        {
            // Arrange
            var query = $"BODY[{section}]".AsAsciiSpan();
            var expected = new string[] { $"BODY[{section}]" };
            // Act
            var item = DataItemParser.Parse(query);
            // Assert
            var actual = new List<string>();
            GetItemNames(actual, item);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void TestMultiple()
        {
            // Arrange
            var query = "(FLAGS RFC822.SIZE ENVELOPE)".AsAsciiSpan();
            var expected = new string[] { "FLAGS", "RFC822.SIZE", "ENVELOPE" };
            // Act
            var item = DataItemParser.Parse(query);
            // Assert
            var actual = new List<string>();
            GetItemNames(actual, item);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        private void GetItemNames(List<string> names, DataItem item)
        {
            if (item is AggregatedDataItem)
            {
                var aggregated = (AggregatedDataItem)item;
                GetItemNames(names, aggregated.Left);
                GetItemNames(names, aggregated.Right);
            } else if (item != null)
            {
                names.Add(item.Name.AsString());
            }
        }
    }
}
