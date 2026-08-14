using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tradelens.Cli.Commands;
using Tradelens.Infrastructure.Clients.Finnhub;

// https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection/usage

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddScoped<IFinnhubClient, FinnhubClient>();
builder.Services.AddHttpClient("Finnhub", client =>
{
    client.BaseAddress = new Uri("https://finnhub.io/api/v1/stock/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<EdgarFactsCommand>();

using IHost host = builder.Build();

var command = host.Services.GetRequiredService<EdgarFactsCommand>();

if (args[0] == "--edgarfacts" && args.Length == 2)
{
    var ticker = args[1];
    var exitCode = await command.ExecuteAsync(ticker);
    return exitCode;
}

return 1;

// public static class Program
// {
//     private static int Main(string[] args)
//     {
//         
//         foreach (var arg in args)
//         {
//             Console.WriteLine(arg);
//         }
//
//         if (args[0] != "--edgarfacts")
//         {
//             Console.WriteLine("Get Edgar company facts.");
//             return 1;
//         }
//
//         if (args.Length != 2)
//         {
//             Console.WriteLine("Format: --edgarfacts <ticker>");
//             return 1;
//         }
//
//         if (args[0] == "--edgarfacts" && args[1] is not null)
//         {
//             var ticker = args[1];
//             Console.WriteLine("Your input is clean, you are getting Edgar facts. You entered the ticker: " + ticker);
//             return 0;
//         }
//         
//         Console.WriteLine("Incorrect input");
//         return 1;
//     }
// }