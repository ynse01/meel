using System;
using System.Collections.Generic;

namespace Meel
{
    public abstract class Metadata
    {
        private Dictionary<string, object> metadata = new Dictionary<string, object>();

        public bool TryGetMetadata<T>(string key, out T value) where T: class
        {
            bool result;
            if (metadata.TryGetValue(key, out object obj))
            {
                value = obj as T;
                result = true;
            }
            else
            {
                value = null;
                result = false;
            }
            return result;
        }

        public bool ContainsMetadata(string key)
        {
            return metadata.ContainsKey(key);
        }

        public void SetMetadata(string key, object value)
        {
            metadata[key] = value;
        }

        public void RemoveMetadata(string key)
        {
            metadata.Remove(key);
        }

        protected void Foreach(Action<string, object> callback)
        {
            foreach(var pair in metadata)
            {
                callback(pair.Key, pair.Value);
            }
        }
    }
}
