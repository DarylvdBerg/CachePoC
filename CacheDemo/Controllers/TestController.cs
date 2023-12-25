using Cache;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
using Microsoft.AspNetCore.Mvc;

namespace CacheDemo;

[ApiController]
[Route("api/v1/[controller]")]
public class TestController : ControllerBase
{
    private readonly IServiceProvider _provider;
    public TestController(IServiceProvider provider) {
        _provider = provider;
    }

    [HttpGet]
    [Route("test")]
    public IProduct Get() {
        // Get required key services.
        var memoryCache = _provider.GetRequiredKeyedService<ICacheService>("memory");
        var redisCache = _provider.GetRequiredKeyedService<ICacheService>("redis");
        Console.WriteLine("TEST");
        // Product key to search by.
        var key = "Product.ababa";

        // Get from cache(s)
        var result = memoryCache.Get(key, () => {
            return redisCache.Get(key, () => {
                // Create product hard coded, but this is where you want to call the source system.
                return new Product() {
                    Id = "ababa",
                    Name = "Test",
                    Price = 14
                };
            });
        });

        // Return result.
        return result;
    }
}
