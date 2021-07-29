using System;

namespace Meel
{
    public abstract class Metadata
    {
        private object[] metadata;

        public Metadata()
        {
            metadata = new object[MetadataKey.MaxId];
        }

        public bool TryGetMetadata<T>(MetadataKey key, out T value) where T: class
        {
            bool result;
            int index = (int)key;
            if (index < metadata.Length) 
            {
                value = metadata[index] as T;
                result = true;
            }
            else
            {
                value = null;
                result = false;
            }
            return result;
        }

        public bool ContainsMetadata(MetadataKey key)
        {
            var index = (int)key;
            return ((index < metadata.Length) && metadata[index] != null);
        }

        public void SetMetadata(MetadataKey key, object value)
        {
            int index = (int)key;
            if (index < metadata.Length)
            {
                metadata[index] = value;
            }
        }

        public void RemoveMetadata(MetadataKey key)
        {
            SetMetadata(key, null);
        }

        protected void Foreach(Action<object> callback)
        {
            foreach(var obj in metadata)
            {
                callback(obj);
            }
        }
    }
}
