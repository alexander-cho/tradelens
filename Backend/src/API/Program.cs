using API.Middleware;
using Core.Interfaces;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

// register Db Context
builder.Services.AddDbContext<TradeLensDbContext>(opt => {
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IPostRepository, PostRepository>();

builder.Services.AddScoped<IPolygonService, PolygonService>();

// type of entity to be used with generic repositories is unknown at this point- typeof()
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// authentication with Identity
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<TradeLensDbContext>();

// CORS
builder.Services.AddCors();

// HTTP Client factory
builder.Services.AddHttpClient("Polygon", client =>
{
    client.BaseAddress = new Uri("https://api.polygon.io/");
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
    var context = services.GetRequiredService<TradeLensDbContext>();
    await context.Database.MigrateAsync();
    await DbContextSeed.SeedPostsAsync(context);
    await DbContextSeed.SeedCompanyData(context);
}
catch (Exception exception)
{
    Console.WriteLine(exception);
    throw;
}

app.Run();
