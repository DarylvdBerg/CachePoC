
using Newtonsoft.Json;

namespace Cache;

public class RedisCache : ICacheService
{
    private readonly ICacheImplementation _impl;

    public RedisCache(ICacheImplementation impl) 
    {
        _impl = impl;
    }

    public void Add(ICacheEntry key, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key.CacheKey);
        _impl.Set(key.CacheKey, value);
    }

    public TResult Get<TResult>(ICacheEntry key, Func<TResult> callback) where TResult : ICacheEntry
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key.CacheKey);
        var result = _impl.Get(key.CacheKey);
        if(string.IsNullOrWhiteSpace(result)) {
            Console.WriteLine("NO HIT IN REDIS");
            var fromCache = callback.Invoke();
            var serialized = JsonConvert.SerializeObject(fromCache);
            _impl.Set(key.CacheKey, serialized);
            return fromCache;
        }

        Console.WriteLine("HIT FROM REDIS CACHE");
        var deserialized = JsonConvert.DeserializeObject<TResult>(result);

        if(deserialized is null) 
        {
            throw new InvalidOperationException("Result was found but was null;");
        }

        return deserialized;
    }
}
