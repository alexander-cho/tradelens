using Tradelens.Api.Extensions;
using Tradelens.Api.Middleware;
using Tradelens.Core.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors();

builder.Logging.AddLogging();

builder.Services.AddDataClients();

builder.Services.AddBusinessLogicServices();

builder.Services.AddPostgresqlDbContext(builder.Configuration);

builder.Services.AddRedis(builder.Configuration);

builder.Services.AddRepositories();

builder.Services.AddIdentityConfiguration();

builder.Services.AddHttpClients(builder.Configuration);


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

// if Tradelens.Api server cannot handle request, gets passed to client app
app.MapFallbackToController("Index", "Fallback");

await app.MigrateAndSeedDatabaseAsync();

app.Run();

public partial class Program {}
