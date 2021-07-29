using System.Threading;

namespace Meel
{
    public struct MetadataKey
    {
        private static int nextId = 0;
        private int id;

        public static MetadataKey Create()
        {
            var key = new MetadataKey();
            key.id = Interlocked.Increment(ref nextId);
            return key;
        }

        public static int MaxId => nextId;

        public static explicit operator int(MetadataKey key)
        {
            return key.id;
        }
    }
}
