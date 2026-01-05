using Tradelens.Core.Interfaces;
using Tradelens.Core.Models;
using Tradelens.Infrastructure.Clients.Fmp;
using Tradelens.Infrastructure.Mappers;

namespace Tradelens.Infrastructure.Services;

public class CongressService : ICongressService
{
    private readonly IFmpClient _fmpClient;

    public CongressService(IFmpClient fmpClient)
    {
        this._fmpClient = fmpClient;
    }

    public async Task<IEnumerable<CongressTradeModel>> GetCongressTradesAsync(string chamber)
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

        switch (chamber)
        {
            case "house":
                var houseTradesDto = await _fmpClient.GetLatestHouseTradesAsync();
                return houseTradesDto.Select(trade => CongressTradeMapper.ToCongressTradesDomainModel(trade)).ToList();
            case "senate":
                var senateTradesDto = await _fmpClient.GetLatestSenateTradesAsync();
                return senateTradesDto.Select(CongressTradeMapper.ToCongressTradesDomainModel).ToList();
            default:
                throw new ArgumentException("invalid congressional chamber", nameof(chamber));
        }
    }
}