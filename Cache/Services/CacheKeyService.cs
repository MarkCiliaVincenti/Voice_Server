using System.Globalization;
using System.Text;

namespace Cache.Services
{
    public abstract class CacheKeyService
    {
        #region Constants

        private string HashAlgorithm => "SHA1";

        protected CacheKeyService()
        {
        }

        #endregion

        #region Utilities

        protected string PrepareKeyPrefix(string prefix, params object[] prefixParameters)
        {
            return prefixParameters?.Any() ?? false
                ? string.Format(prefix, prefixParameters.Select(CreateCacheKeyParameters).ToArray())
                : prefix;
        }

        protected string CreateIdsHash(IEnumerable<long> ids)
        {
            var identifiers = ids.ToList();

            if (!identifiers.Any())
                return string.Empty;

            var identifiersString = string.Join(", ", identifiers.OrderBy(id => id));
            return HashHelper.CreateHash(Encoding.UTF8.GetBytes(identifiersString), HashAlgorithm);
        }

        protected object CreateCacheKeyParameters(object parameter)
        {
            switch (parameter)
            {
                case null:
                    return "null";
                case IEnumerable<long> ids:
                    return CreateIdsHash(ids);
                case decimal param:
                    return param.ToString(CultureInfo.InvariantCulture);
                default:
                    return parameter;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a copy of cache key and fills it by passed parameters
        /// </summary>
        /// <param name="cacheKey">Initial cache key</param>
        /// <param name="cacheKeyParameters">Parameters to create cache key</param>
        /// <returns>Cache key</returns>
        public virtual CacheKey PrepareKey(CacheKey cacheKey, params object[] cacheKeyParameters)
        {
            return cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);
        }

        public virtual CacheKey PrepareKeyForDefaultCache(CacheKey cacheKey, params object[] cacheKeyParameters)
        {
            var key = cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);
            key.CacheTime = CacheSettings.CacheConfig.DefaultCacheTime;

            return key;
        }

        public virtual CacheKey PrepareKeyForShortTermCache(CacheKey cacheKey, params object[] cacheKeyParameters)
        {
            var key = cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);

            key.CacheTime = CacheSettings.CacheConfig.ShortTermCacheTime;

            return key;
        }

        #endregion
    }
}