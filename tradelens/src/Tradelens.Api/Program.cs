using Tradelens.Api.Extensions;
using Tradelens.Api.Middleware;
using Tradelens.Core.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .Services
    .AddCors()
    .AddDataClients()
    .AddBusinessLogicServices()
    .AddPostgresqlDbContext(builder.Configuration)
    .AddRedis(builder.Configuration)
    .AddRepositories()
    .AddIdentityConfiguration()
    .AddHttpClients(builder.Configuration);

builder.Logging.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseMiddleware<ExceptionMiddleware>();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

// get access to provided Identity endpoints
// overwrite url so it's not {{url}}/login, but rather {{url}}/api/login
app.MapGroup("api").MapIdentityApi<User>();

// if Api server cannot handle request, gets passed to client app
app.MapFallbackToController("Index", "Fallback");

await app.MigrateAndSeedDatabaseAsync();

app.Run();

public partial class Program {}
