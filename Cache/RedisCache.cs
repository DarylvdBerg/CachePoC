
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Cache;

internal sealed class RedisCache : ICacheService
{
    private readonly IDatabase _db;

    public RedisCache() 
    {
        var redis = ConnectionMultiplexer.Connect("localhost");
        _db = redis.GetDatabase();
    }

    public void Add(string key, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        _db.StringSet(key, value, expiry: TimeSpan.FromMinutes(10));
    }

    public TResult Get<TResult>(string key, Func<TResult> callback) where TResult : ICacheEntry
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        var result = _db.StringGet(key);
        if(string.IsNullOrWhiteSpace(result)) 
        {
            Console.WriteLine("NO HIT IN REDIS");
            var fromCache = callback.Invoke();
            var serialized = JsonConvert.SerializeObject(fromCache);
            ArgumentException.ThrowIfNullOrWhiteSpace(fromCache.CacheKey);
            Add(fromCache.CacheKey, serialized);
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
