using API.Middleware;
using Core.Interfaces;
using Core.Entities;
using Infrastructure.Clients;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using OpenTelemetry.Logs;
using StackExchange.Redis;
using Polly;
using Polly.Retry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// register Db Context
builder.Services.AddDbContext<TradelensDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// logging
builder.Logging.ClearProviders();
builder.Logging.AddOpenTelemetry(logging => logging.AddConsoleExporter());

builder.Services.AddScoped<IPostRepository, PostRepository>();

builder.Services.AddScoped<IPolygonService, PolygonService>();
builder.Services.AddScoped<IPolygonClient, PolygonClient>();
builder.Services.AddScoped<IFmpService, FmpService>();
builder.Services.AddScoped<IFmpClient, FmpClient>();
builder.Services.AddScoped<IFinnhubClient, FinnhubClient>();
builder.Services.AddScoped<IFinnhubService, FinnhubService>();

// redis caching service functionality is a singleton so users can access same instance
builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();

// type of entity to be used with generic repositories is unknown at this point- typeof()
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// authentication with Identity
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<TradelensDbContext>();

// CORS
builder.Services.AddCors();

// Redis, singleton so it's alive for the duration of the application
builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
{
    var connectionString = builder.Configuration.GetConnectionString("Redis");
    if (connectionString == null)
    {
        throw new Exception("Cannot get Redis connection string");
    }

    var configuration = ConfigurationOptions.Parse(connectionString, ignoreUnknown: true);
    return ConnectionMultiplexer.Connect(configuration);
});

// HTTP Client factory
builder.Services.AddHttpClient("Polygon", client =>
{
    client.BaseAddress = new Uri("https://api.polygon.io/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient("Fmp", client =>
{
    // client.BaseAddress = new Uri("https://financialmodelingprep.com/stable/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient("Finnhub", client =>
{
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseMiddleware<ExceptionMiddleware>();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

// get access to provided Identity endpoints
// overwrite url so it's not {{url}}/login, but api is there
app.MapGroup("api").MapIdentityApi<User>();

// if API server cannot handle request, gets passed to client app
app.MapFallbackToController("Index", "Fallback");

// re-seeding the db with data when restarting the application server
try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TradelensDbContext>();
    
    // retry policy for Npgsql exceptions
    AsyncRetryPolicy retryPolicy = Policy
        .Handle<NpgsqlException>()
        .WaitAndRetryAsync(
            retryCount: 5,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (exception, retryCount, context) =>
            {
                Console.WriteLine($"Retry {retryCount} due to {exception.Message}");
            });

    await retryPolicy.ExecuteAsync(async () =>
    {
        await context.Database.MigrateAsync();
    });
    
    await DbContextSeed.SeedPostsAsync(context);
    await DbContextSeed.SeedCompanyData(context);
}
catch (Exception exception)
{
    Console.WriteLine(exception);
    throw;
}

app.Run();

public partial class Program {}
