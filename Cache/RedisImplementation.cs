using StackExchange.Redis;

namespace Cache;

public class RedisImplementation : ICacheImplementation
{
    private readonly IDatabase _db;

    public RedisImplementation() {
        var redis = ConnectionMultiplexer.Connect("localhost");
        _db = redis.GetDatabase();
    }

    public string Get(string key)
    {
        var result = _db.StringGet(key);
        if(string.IsNullOrWhiteSpace(result)) 
        {
            return string.Empty;
        }

        return result;
    }

    public void Set(string key, string value)
    {
        _db.StringSet(key, value, expiry: TimeSpan.FromMinutes(10));
    }
}
