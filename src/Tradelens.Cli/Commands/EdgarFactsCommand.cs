using Tradelens.Infrastructure.Clients.Finnhub;

namespace Tradelens.Cli.Commands;

public class EdgarFactsCommand(IFinnhubClient client)
{
    // private readonly IFinnhubClient _client;
    //
    // public EdgarFactsCommand(IFinnhubClient client)
    // {
    //     _client = client;
    // }

    public async Task<int> ExecuteAsync(string ticker)
    {
        var result = await client.GetSecFilingsAsync(ticker);
        var cikResult = result?[0].Cik;
        if (cikResult != null)
        {
            // works, move to Infrastructure
            var companyFactsUri = "https://data.sec.gov/api/xbrl/companyfacts/CIK" + AddZerosToCik(cikResult) + ".json";
            Console.WriteLine(companyFactsUri);
        }
        return 0;
    }
    
    /// <summary>
    /// Add however many 0's need to get to length 10 for full cik. SEC endpoints require full Central Index Key (CIK).
    /// </summary>
    /// <remarks>
    /// https://stackoverflow.com/questions/23846117/is-there-built-in-method-to-add-character-multiple-times-to-a-string</remarks>
    /// <param name="cik">string cik returned from Finnhub SEC endpoint</param>
    /// <returns>string: full cik</returns>
    private static string AddZerosToCik(string cik)
    {
        var zerosToAdd = 10 - cik.Length;
        return cik.PadLeft(zerosToAdd + cik.Length, '0');
    }
}