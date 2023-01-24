using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Nito.AsyncEx;

namespace Cache.Services
{
    public partial class DistributedCacheManager : CacheKeyService, ILocker, IStaticCacheManager
    {
        #region Fields

        private readonly IDistributedCache _distributedCache;
        private readonly DistributedCacheConfig _distributedCacheConfig;
        private readonly PerRequestCache _perRequestCache;
        private static List<string> _keys;
        private static readonly AsyncLock Locker;

        #endregion

        #region Ctor

        static DistributedCacheManager()
        {
            Locker = new AsyncLock();
            _keys = new List<string>();
        }

        public static void SetKeys(List<string> keys)
        {
            _keys = keys;
        }

        public DistributedCacheManager(IDistributedCache distributedCache,
            DistributedCacheConfig distributedCacheConfig,
            IHttpContextAccessor httpContextAccessor)
        {
            _distributedCache = distributedCache;
            _distributedCacheConfig = distributedCacheConfig;
            _perRequestCache = new PerRequestCache(httpContextAccessor);
            // _keys = GetRedisKeys();
        }

        #endregion

        #region Utilities

        private DistributedCacheEntryOptions PrepareEntryOptions(CacheKey key)
        {
            //set expiration time for the passed cache key
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(key.CacheTime)
            };

            return options;
        }

        private async Task<(bool isSet, T item)> TryGetItemAsync<T>(CacheKey key)
        {
            var json = await _distributedCache.GetStringAsync(key.Key);

            if (string.IsNullOrEmpty(json))
                return (false, default);

            var item = JsonConvert.DeserializeObject<T>(json);
            _perRequestCache.Set(key.Key, item);

            return (true, item);
        }

        private (bool isSet, T item) TryGetItem<T>(CacheKey key)
        {
            var json = _distributedCache.GetString(key.Key);

            if (string.IsNullOrEmpty(json))
                return (false, default);

            var item = JsonConvert.DeserializeObject<T>(json);
            _perRequestCache.Set(key.Key, item);

            return (true, item);
        }

        private void Set(CacheKey key, object data)
        {
            if ((key?.CacheTime ?? 0) <= 0 || data == null)
                return;

            _distributedCache.SetString(key.Key, JsonConvert.SerializeObject(data), PrepareEntryOptions(key));
            _perRequestCache.Set(key.Key, data);

            using var _ = Locker.Lock();
            _keys.Add(key.Key);
        }

        #endregion

        #region Methods

        public void Dispose()
        {
        }

        public async Task<T> GetAsync<T>(CacheKey key, Func<Task<T>> acquire)
        {
            //little performance workaround here:
            //we use "PerRequestCache" to cache a loaded object in memory for the current HTTP request.
            //this way we won't connect to Redis server many times per HTTP request (e.g. each time to load a locale or setting)
            if (_perRequestCache.IsSet(key.Key))
                return _perRequestCache.Get(key.Key, () => default(T));

            if (key.CacheTime <= 0)
                return await acquire();

            var (isSet, item) = await TryGetItemAsync<T>(key);

            if (isSet)
                return item;

            var result = await acquire();

            if (result != null)
                await SetAsync(key, result);

            return result;
        }

        public async Task<T> GetAsync<T>(CacheKey key, Func<T> acquire)
        {
            //little performance workaround here:
            //we use "PerRequestCache" to cache a loaded object in memory for the current HTTP request.
            //this way we won't connect to Redis server many times per HTTP request (e.g. each time to load a locale or setting)
            if (_perRequestCache.IsSet(key.Key))
                return _perRequestCache.Get(key.Key, () => default(T));

            if (key.CacheTime <= 0)
                return acquire();

            var (isSet, item) = await TryGetItemAsync<T>(key);

            if (isSet)
                return item;

            var result = acquire();

            if (result != null)
                await SetAsync(key, result);

            return result;
        }


        public T Get<T>(CacheKey key, Func<T> acquire)
        {
            //little performance workaround here:
            //we use "PerRequestCache" to cache a loaded object in memory for the current HTTP request.
            //this way we won't connect to Redis server many times per HTTP request (e.g. each time to load a locale or setting)
            if (_perRequestCache.IsSet(key.Key))
                return _perRequestCache.Get(key.Key, () => default(T));

            if (key.CacheTime <= 0)
                return acquire();

            var (isSet, item) = TryGetItem<T>(key);

            if (isSet)
                return item;

            var result = acquire();

            if (result != null)
                Set(key, result);

            return result;
        }

        /// <summary>
        /// Remove the value with the specified key from the cache
        /// </summary>
        /// <param name="cacheKey">Cache key</param>
        /// <param name="cacheKeyParameters">Parameters to create cache key</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task RemoveAsync(CacheKey cacheKey, params object[] cacheKeyParameters)
        {
            cacheKey = PrepareKey(cacheKey, cacheKeyParameters);

            await _distributedCache.RemoveAsync(cacheKey.Key);
            _perRequestCache.Remove(cacheKey.Key);

            using var _ = await Locker.LockAsync();
            _keys.Remove(cacheKey.Key);
        }

        /// <summary>
        /// Add the specified key and object to the cache
        /// </summary>
        /// <param name="key">Key of cached item</param>
        /// <param name="data">Value for caching</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task SetAsync(CacheKey key, object data)
        {
            if ((key?.CacheTime ?? 0) <= 0 || data == null)
                return;

            await _distributedCache.SetStringAsync(key.Key, JsonConvert.SerializeObject(data),
                PrepareEntryOptions(key));
            _perRequestCache.Set(key.Key, data);

            using var _ = await Locker.LockAsync();
            _keys.Add(key.Key);
        }

        public async Task RemoveByPrefixAsync(string prefix, params object[] prefixParameters)
        {
            prefix = PrepareKeyPrefix(prefix, prefixParameters);
            _perRequestCache.RemoveByPrefix(prefix);

            using var _ = await Locker.LockAsync();

            foreach (var key in _keys.Where(key => key.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
                         .ToList())
            {
                await _distributedCache.RemoveAsync(key);
                _keys.Remove(key);
            }
        }

        public async Task ClearAsync()
        {
            //we can't use _perRequestCache.Clear(),
            //because HttpContext stores some server data that we should not delete
            foreach (var redisKey in _keys)
                _perRequestCache.Remove(redisKey);

            using var _ = await Locker.LockAsync();

            foreach (var key in _keys)
                await _distributedCache.RemoveAsync(key);

            _keys.Clear();
        }

        public void PerformActionWithLock(string key, TimeSpan? expirationTime, Action action,
            bool throwIfLocked = false)
        {
            //ensure that lock is acquired
            if (IsActionLocked(key))
                if (throwIfLocked) throw new Exception($"Operation with key : '{key}' is locked");
                else return;

            try
            {
                Lock(key, expirationTime);
                //perform action
                action();
                _distributedCache.Remove(key);
            }
            catch (Exception)
            {
                //release lock even if action fails
                _distributedCache.Remove(key);
                throw;
            }
        }

        /// <inheritdoc cref="ILocker.PerformAsyncActionWithLock"/>
        public async Task PerformAsyncActionWithLock(string key, TimeSpan? expirationTime, Func<Task> action,
            bool throwIfLocked = false)
        {
            if (IsActionLocked(key))
                if (throwIfLocked) throw new Exception($"Operation with key : '{key}' is locked");
                else return;
            try
            {
                await LockAsync(key, expirationTime);
                //perform action
                await action();
                await _distributedCache.RemoveAsync(key);
            }
            catch (Exception)
            {
                //release lock even if action fails
                await _distributedCache.RemoveAsync(key);
                throw;
            }
        }

        #endregion

        #region Nested class

        protected class PerRequestCache
        {
            #region Fields

            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly ReaderWriterLockSlim _lockSlim;

            #endregion

            #region Ctor

            public PerRequestCache(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;

                _lockSlim = new ReaderWriterLockSlim();
            }

            #endregion

            #region Utilities

            protected virtual IDictionary<object, object> GetItems()
            {
                return _httpContextAccessor.HttpContext?.Items;
            }

            #endregion

            #region Methods

            public virtual T Get<T>(string key, Func<T> acquire)
            {
                IDictionary<object, object> items;

                using (new ReaderWriteLockDisposable(_lockSlim, ReaderWriteLockType.Read))
                {
                    items = GetItems();
                    if (items == null)
                        return acquire();

                    //item already is in cache, so return it
                    if (items[key] != null)
                        return (T) items[key];
                }

                //or create it using passed function
                var result = acquire();

                //and set in cache (if cache time is defined)
                using (new ReaderWriteLockDisposable(_lockSlim))
                    items[key] = result;

                return result;
            }

            public virtual void Set(string key, object data)
            {
                if (data == null)
                    return;

                using (new ReaderWriteLockDisposable(_lockSlim))
                {
                    var items = GetItems();
                    if (items == null)
                        return;

                    items[key] = data;
                }
            }

            /// <summary>
            /// Get a value indicating whether the value associated with the specified key is cached
            /// </summary>
            /// <param name="key">Key of cached item</param>
            /// <returns>True if item already is in cache; otherwise false</returns>
            public virtual bool IsSet(string key)
            {
                using (new ReaderWriteLockDisposable(_lockSlim, ReaderWriteLockType.Read))
                {
                    var items = GetItems();
                    return items?[key] != null;
                }
            }

            public virtual void Remove(string key)
            {
                using (new ReaderWriteLockDisposable(_lockSlim))
                {
                    var items = GetItems();
                    items?.Remove(key);
                }
            }

            public virtual void RemoveByPrefix(string prefix)
            {
                using (new ReaderWriteLockDisposable(_lockSlim, ReaderWriteLockType.UpgradeableRead))
                {
                    var items = GetItems();
                    if (items == null)
                        return;

                    //get cache keys that matches pattern
                    var regex = new Regex(prefix,
                        RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    var matchesKeys = items.Keys.Select(p => p.ToString())
                        .Where(key => regex.IsMatch(key ?? string.Empty)).ToList();

                    if (!matchesKeys.Any())
                        return;

                    using (new ReaderWriteLockDisposable(_lockSlim))
                        //remove matching values
                        foreach (var key in matchesKeys)
                            items.Remove(key);
                }
            }

            #endregion
        }

        #endregion

        public void Lock(string key, TimeSpan? expirationTime)
        {
            if (expirationTime != null)
            {
                _distributedCache.SetString(key, key, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expirationTime
                });
            }
            else
            {
                _distributedCache.SetString(key, key);
            }
        }

        public async Task LockAsync(string key, TimeSpan? expirationTime)
        {
            if (expirationTime != null)
            {
                await _distributedCache.SetStringAsync(key, key, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expirationTime
                });
            }
            else
            {
                await _distributedCache.SetStringAsync(key, key);
            }
        }

        public async Task UnlockAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);
        }

        public void Unlock(string key)
        {
            _distributedCache.Remove(key);
        }

        public bool IsActionLocked(string key)
        {
            var cached = _distributedCache.GetString(key);
            if (string.IsNullOrEmpty(cached))
                return false;
            return true;
        }
    }
}