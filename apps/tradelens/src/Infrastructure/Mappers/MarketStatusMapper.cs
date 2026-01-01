using Core.Models;
using Infrastructure.Clients.Finnhub.DTOs;

namespace Infrastructure.Mappers;

public static class MarketStatusMapper
{
    public static MarketStatusModel ToMarketStatusDomainModel(MarketStatusDto marketStatusDto)
    {
        return new MarketStatusModel
        {
            Exchange = marketStatusDto.Exchange,
            Holiday = marketStatusDto.Holiday,
            IsOpen = marketStatusDto.IsOpen,
            Session = marketStatusDto.Session,
            Timezone = marketStatusDto.Timezone,
            T = marketStatusDto.T
        };
    }
}