using API.Extensions;
using API.Middleware;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Polly;
using Polly.Retry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors();

builder.Logging.AddLogging();

builder.Services.AddDataServices();

builder.Services.AddPostgresqlDbContext(builder.Configuration);

builder.Services.AddRedis(builder.Configuration);

builder.Services.AddRepositories();

builder.Services.AddIdentityConfiguration();

builder.Services.AddHttpClients();


var app = builder.Build();

// Configure the HTTP request pipeline.

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

public static partial class Program {}
