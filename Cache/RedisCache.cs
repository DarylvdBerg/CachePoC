
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Cache;

public class RedisCache : ICacheService
{
    private readonly IDatabase _db;

    public RedisCache() 
    {
        var redis = ConnectionMultiplexer.Connect("localhost");
        _db = redis.GetDatabase();
    }

    public void Add(ICacheEntry key, string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key.CacheKey);
        _db.StringSet(key.CacheKey, value, expiry: TimeSpan.FromMinutes(10));
    }

    public TResult Get<TResult>(ICacheEntry key, Func<TResult> callback) where TResult : ICacheEntry
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key.CacheKey);
        var result = _db.StringGet(key.CacheKey);
        if(string.IsNullOrWhiteSpace(result)) 
        {
            Console.WriteLine("NO HIT IN REDIS");
            var fromCache = callback.Invoke();
            var serialized = JsonConvert.SerializeObject(fromCache);
            Add(fromCache, serialized);
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
