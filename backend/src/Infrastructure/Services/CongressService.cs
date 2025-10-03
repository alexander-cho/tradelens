using Core.Interfaces;
using Infrastructure.Clients.Fmp;

namespace Infrastructure.Services;

public class CongressService : ICongressService
{
    private readonly IFmpClient _fmpClient;

    public CongressService(IFmpClient fmpClient)
    {
        this._fmpClient = fmpClient;
    }

    public async Task<IEnumerable<string>> GetCongressTradesAsync(string chamber)
    {
        // if (chamber == "house")
        // {
        //     return await _fmpClient.GetLatestHouseTradesAsync();
        // }
        //
        // if (chamber == "senate")
        // {
        //     return await _fmpClient.GetLatestSenateTradesAsync();
        // }
        //
        // throw new Exception("invalid congressional chamber");

        // switch (chamber)
        // {
        //     case "house":
        //         return await _fmpClient.GetLatestHouseTradesAsync();
        //     case "senate":
        //         return await _fmpClient.GetLatestSenateTradesAsync();
        //     default:
        //         throw new ArgumentException("invalid congressional chamber", nameof(chamber));
        // }

        throw new NotImplementedException();
    }
}