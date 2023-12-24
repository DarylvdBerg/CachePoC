namespace Cache;

public interface ICacheService
{
    TResult Get<TResult>(ICacheEntry key, Func<TResult> callback) where TResult : ICacheEntry;
    void Add(ICacheEntry key, string value);
}
