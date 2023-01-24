using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cache
{
    public abstract partial class DistributedCacheConfig
    {
        /// <summary>
        /// Gets or sets a distributed cache type
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CacheType CacheType { get; set; } = CacheType.Memory;

        /// <summary>
        /// Gets or sets connection string. Used when distributed cache is enabled
        /// </summary>
        public string ConnectionString { get; set; } = "127.0.0.1:6379,ssl=False";
    }

    /// <summary>
    /// Represents distributed cache types enumeration
    /// </summary>
    public enum CacheType
    {
        [EnumMember(Value = "memory")] Memory,
    }
}