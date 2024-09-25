// using Microsoft.EntityFrameworkCore;
// using TradeLens.Models;

// var builder = WebApplication.CreateBuilder(args);

// // Add services to the container.
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddControllers();
// builder.Services.AddDbContext<PostContext>(opt =>
//     opt.UseInMemoryDatabase("Posts"));
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
// builder.Services.AddHttpClient();

// var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.MapControllers();

// app.Run();


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;

namespace TradeLens
{
    class Program
    {
        static HttpClient _httpClient = new HttpClient();
        public static async Task Main(string[] args)
        {
            await Polygon.GetBars(_httpClient);
        }

        public class Polygon
        {
            public static async Task GetBars(HttpClient _httpClient)
            {
                using HttpResponseMessage response = await _httpClient.GetAsync("https://api.polygon.io/v2/aggs/ticker/SOFI/range/1/day/2024-01-01/2024-09-23?adjusted=true&sort=asc&apiKey=UQE0AfiLy65mtQuTOX9mpQZqWV_Qb8iP");
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"{jsonResponse}\n");
            }
        }
    }
}
