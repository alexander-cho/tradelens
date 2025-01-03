using API.Middleware;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
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

// type of entity to be used with generic repositories is unknown at this point- typeof()
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// CORS
builder.Services.AddCors();

var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.MapOpenApi();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200"));
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

// re-seeding the db with data when restarting the application server
try
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TradeLensDbContext>();
    await context.Database.MigrateAsync();
    await DbContextSeed.SeedPostsAsync(context);
}
catch (Exception exception)
{
    Console.WriteLine(exception);
    throw;
}

app.Run();
