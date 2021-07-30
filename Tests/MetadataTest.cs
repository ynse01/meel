using NUnit.Framework;

namespace Meel.Tests
{
    [TestFixture]
    public class MetadataTest
    {
        private MetadataKey firstKey;
        private MetadataKey secondKey;
        private MetadataKey thirdKey;

        public class MetadataForTest : Metadata { }

        public MetadataTest()
        {
            firstKey = MetadataKey.Create();
            secondKey = MetadataKey.Create(); 
            thirdKey = MetadataKey.Create();
        }

        [Test]
        public void TryGetMetadataShouldReturnSameObject()
        {
            // Arrange
            var metadata = new MetadataForTest();
            var expected = "Expected";
            metadata.SetMetadata(firstKey, expected);
            // Act
            var result = metadata.TryGetMetadata<string>(firstKey, out string actual);
            // Assert
            Assert.AreEqual(4, MetadataKey.MaxId);
            Assert.IsTrue(result);
            Assert.AreSame(expected, actual);
        }

        [Test]
        public void TryGetMetadataShouldFailOnEmptySlot()
        {
            // Arrange
            var metadata = new MetadataForTest();
            // Act
            var result = metadata.TryGetMetadata<string>(firstKey, out string actual);
            // Assert
            Assert.AreEqual(3, MetadataKey.MaxId);
            Assert.IsFalse(result);
            Assert.IsNull(actual);
        }

        [Test]
        public void TryGetMetadataShouldFailOnNonExistingSlot()
        {
            // Arrange
            var metadata = new MetadataForTest();
            var nonExistingKey = MetadataKey.Create();
            // Act
            var result = metadata.TryGetMetadata<string>(nonExistingKey, out string actual);
            // Assert
            Assert.AreEqual(4, MetadataKey.MaxId);
            Assert.IsFalse(result);
            Assert.IsNull(actual);
        }
    }
}
