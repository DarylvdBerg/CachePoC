using System.Runtime.CompilerServices;
using Cache;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        ConfigureServices(builder.Services);
        RegisterCache(builder.Services);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.MapControllers();
        app.Run();
    }

    private static void ConfigureServices(IServiceCollection services) {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddMemoryCache();
    }

    private static void RegisterCache(IServiceCollection services) {
        services.AddKeyedSingleton<ICacheService, MemoryCache>("memory");
        services.AddKeyedSingleton<ICacheService, RedisCache>("redis");
    }
}