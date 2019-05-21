using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalCache
{
    /// <summary>
    /// Does a minimal caching.
    /// </summary>
    public class Cache : ICache
    {
        Dictionary<string, object> Objects { get; set; }
        private object _lock { get; set; }

        public Cache()
        {
            _lock = new Object();
            Objects = new Dictionary<string, object>();
        }

        /// <summary>
        /// Adds to cache.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add<T>(string key, T value) where T: class
        {
            lock(_lock)
            {
                Objects.Add(key, value);
            }
        }

        /// <summary>
        /// Creates an instance of object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="constructorParameters"></param>
        /// <returns></returns>
        public T CreateInstance<T>(string key, params object[] constructorParameters) where T : class
        {
            T o = (T)Activator.CreateInstance(typeof(T), constructorParameters);
            lock (_lock)
            {                
                if (!Objects.ContainsKey(key))
                {
                    Objects.Add(key, o);
                }
                
                return o;
            }
        }

        /// <summary>
        /// Gets object by key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T Get<T>(string key) where T: class
        {
            lock(_lock)
            {
                if (Objects.TryGetValue(key, out object value))
                {
                    return (T)value;
                }
            }
            return default;
        }

        /// <summary>
        /// Removes object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        public void Remove<T>(string key) where T : class
        {
            lock(_lock)
            {
                if (Objects.ContainsKey(key))
                {
                    Objects.Remove(key);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }            
        }
    }

    public interface ICache
    {
        T CreateInstance<T>(string key, params object[] constructorParameters) where T : class;
        T Get<T>(string key) where T : class;
        void Add<T>(string key, T value) where T : class;
        void Remove<T>(string key) where T : class;
    }
}
