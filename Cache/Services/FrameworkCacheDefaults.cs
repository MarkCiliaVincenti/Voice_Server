namespace Cache.Services;

public static class FrameworkCacheDefaults<T> where T : class
{
    public static string TypeName => typeof(T).Name.ToLowerInvariant();

    public static CacheKey AllCacheKey => new($"Framework.{TypeName}.all.", AllPrefix, Prefix);
    public static string Prefix => $"Framework.{TypeName}.";
    public static string AllPrefix => $"Framework.{TypeName}.all.";

    public static string ByIdsPrefix => $"Framework.{TypeName}.byids.";
    public static string ByIdPrefix => $"Framework.{TypeName}.byid.";

    public static CacheKey ByIdCacheKey => new CacheKey($"Framework.{TypeName}.byid.{{0}}", ByIdPrefix, Prefix);
}