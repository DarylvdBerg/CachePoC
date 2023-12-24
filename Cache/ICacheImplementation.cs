namespace Cache;

public interface ICacheImplementation
{
    string Get(string key);
    void Set(string key, string value);
}
