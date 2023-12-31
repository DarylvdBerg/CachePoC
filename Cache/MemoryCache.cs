﻿using System.Runtime.Caching;
using System.Runtime.Caching.Hosting;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace Cache;

internal sealed class MemoryCache : ICacheService
{
    private readonly IMemoryCache _cache;
    public MemoryCache(IMemoryCache cache) 
    {
        _cache = cache;
    }

    public void Add(string key, string value)
    {
        ArgumentException.ThrowIfNullOrEmpty(key);
        _cache.Set(key, value, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(300)));
    }

    public TResult Get<TResult>(string key, Func<TResult> callback) where TResult : ICacheEntry
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        var result = _cache.Get<string>(key);
        if(string.IsNullOrWhiteSpace(result)) {
            Console.WriteLine("NO HIT IN MEMORY");
            var fromCache = callback.Invoke();
            var serialized = JsonConvert.SerializeObject(fromCache);
            ArgumentException.ThrowIfNullOrWhiteSpace(fromCache.CacheKey);
            Add(fromCache.CacheKey, serialized);
            return fromCache;
        }

        Console.WriteLine("HIT FROM MEMORY CACHE");
        
        var deserialized = JsonConvert.DeserializeObject<TResult>(result);

        if(deserialized is null) 
        {
            throw new InvalidOperationException("Found object but deserialized null");
        }

        return deserialized;
    }
}
