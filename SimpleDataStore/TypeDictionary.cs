using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotStash
{
    /// <remarks>
    /// This is just a basic 1:1 wrapper around Dictionary for now
    /// Placeholder for optimised implementation later on
    /// </remarks>    
    public class TypeDictionary<TValue> : IDictionary<Type, TValue>
    {
        private readonly Dictionary<Type, TValue> _dictionary;        
        public ICollection<Type> Keys => _dictionary.Keys;
        public ICollection<TValue> Values => _dictionary.Values;

        public int Count => _dictionary.Count;
        public bool IsReadOnly => false;

        public TValue this[Type key]
        {
            get { return _dictionary[key]; }
            set { _dictionary[key] = value; }
        }

        public TValue SafeGet<T>()
        {
            TValue key;
            TryGetValue(typeof(T), out key);
            return key;
        }

        public TValue Get<T>()
        {
            return _dictionary[typeof(T)];
        }

        public void Set<T>(TValue value)
        {
            _dictionary[typeof(T)] = value;
        }

        public TypeDictionary()
        {
            _dictionary = new Dictionary<Type, TValue>();
        }

        public void Add(KeyValuePair<Type, TValue> item)
        {
            _dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<Type, TValue> item)
        {
            return _dictionary.Contains(item);
        }        

        public bool Remove(KeyValuePair<Type, TValue> item)
        {
            return _dictionary.Remove(item.Key);
        }                

        public IEnumerator<KeyValuePair<Type, TValue>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(Type key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void Add(Type key, TValue value)
        {
            _dictionary.Add(key, value);
        }

        public bool Remove(Type key)
        {
            return _dictionary.Remove(key);
        }

        public bool TryGetValue(Type key, out TValue value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        public void CopyTo(KeyValuePair<Type, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException("Don't care");
        }
    }
}
