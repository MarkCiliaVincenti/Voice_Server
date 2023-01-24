using Newtonsoft.Json.Linq;

namespace Cache
{
    /// <summary>
    /// Represents the app settings
    /// </summary>
    public class CacheSettings
    {

        #region Properties

        /// <summary>
        /// Gets or sets cache configuration parameters
        /// </summary>
        public static CacheConfig CacheConfig { get; set; } = new CacheConfig();

        #endregion

        /// <summary>
        /// Gets or sets additional configuration parameters
        /// </summary>
        [Newtonsoft.Json.JsonExtensionData]
        public static IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
