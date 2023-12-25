namespace Cache;

public interface IProduct {
    string? Id {get; }
    string? Name {get;}
    decimal? Price {get;}
}

public class Product : IProduct, ICacheEntry
{
    public string? CacheKey => $"Product.{Id}";

    public string? Id {get; init;}

    public string? Name {get; init;}

    public decimal? Price {get; init;}
}
