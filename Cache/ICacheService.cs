namespace Cache;

public interface ICacheService
{
    TResult Get<TResult>(string key, Func<TResult> callback) where TResult : ICacheEntry;
    void Add(string key, string value);
}
