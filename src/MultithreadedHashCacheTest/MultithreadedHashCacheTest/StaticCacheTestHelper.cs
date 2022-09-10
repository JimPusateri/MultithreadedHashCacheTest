using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadedHashCacheTest
{
    internal static class StaticCacheTestHelper
    {
        public static readonly ConcurrentDictionary<string, object> Cache = new ConcurrentDictionary<string, object>();
    }

    public class SafeDictionary: IDictionary<long, string>
    {
        private DateTime lastPurge = DateTime.Now;
        private int purgeIntervalMs = 3000;

        private readonly object syncRoot = new object();
        private Dictionary<long, string> d = new Dictionary<long, string>();
        private bool purgedEnabled;

        public ICollection<long> Keys => d.Keys;

        public ICollection<string> Values => d.Values;

        public int Count => d.Count;

        public bool IsReadOnly => false;//don't think I care about this in this instance

        public string this[long key] { get => d[key]; set => d[key] = value; }

        public SafeDictionary(bool disablePurge = false):base()
        {
            purgedEnabled = !disablePurge;            
        }
        public void Add(long key, string value)
        {
            throw new NotImplementedException();//we want a try mechanism so this is explicitly not supported
        }
        public bool TryAdd(long key, string value)
        {
            lock (syncRoot)
            {
                if (purgedEnabled && 
                    (DateTime.Now - lastPurge).TotalMilliseconds > purgeIntervalMs)
                {
                    PurgeDictionary();
                    lastPurge = DateTime.Now;
                }
                    
                
                return d.TryAdd(key, value);                
            }
        }

        private void PurgeDictionary()
        {
            var keys = d.Keys.Where(x => x < DateTime.Now.AddMilliseconds(-purgeIntervalMs).ToFileTimeUtc()).ToList();
            foreach (var key in keys)
            {
                d.Remove(key);
            }
        }

        public event EventHandler ItemAdded;

        protected virtual void OnItemAdded(EventArgs e)
        {
            EventHandler handler = ItemAdded;
            if (handler != null)
                handler(this, e);
        }

        public bool ContainsKey(long key)
        {
            return d.ContainsKey(key);
        }

        public bool Remove(long key)
        {
            return d.Remove(key);
        }

        public bool TryGetValue(long key, [MaybeNullWhen(false)] out string value)
        {
            return d.TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<long, string> item)
        {
            d.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            d.Clear();
        }

        public bool Contains(KeyValuePair<long, string> item)
        {
            return d.Contains(item);
        }

        public void CopyTo(KeyValuePair<long, string>[] array, int arrayIndex)
        {
            throw new NotImplementedException();//no need for this use
        }

        public bool Remove(KeyValuePair<long, string> item)
        {
            throw new NotImplementedException(); //no need for this use
        }

        public IEnumerator<KeyValuePair<long, string>> GetEnumerator()
        {
            return d.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return d.GetEnumerator();
        }

        
    }
}
