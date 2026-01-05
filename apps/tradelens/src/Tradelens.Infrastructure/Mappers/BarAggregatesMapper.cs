using Tradelens.Core.Models;
using Tradelens.Infrastructure.Clients.Polygon.DTOs;

namespace Tradelens.Infrastructure.Mappers;

public static class BarAggregatesMapper
{
    public static BarAggregatesModel ToBarAggregateDomainModel(BarAggregatesDto barAggregatesDto)
    {
        return new BarAggregatesModel
        {
            Ticker = barAggregatesDto.Ticker,
            QueryCount = barAggregatesDto.QueryCount,
            ResultsCount = barAggregatesDto.ResultsCount,
            Adjusted = barAggregatesDto.Adjusted,
            Results = barAggregatesDto.Results.Select(ToBarDomainModel).ToList()
        };
    }

    private static Core.Models.Bar ToBarDomainModel(Clients.Polygon.DTOs.Bar bar)
    {
        return new Core.Models.Bar
        {
            V = bar.V,
            Vw = bar.Vw,
            O = bar.O,
            C = bar.C,
            H = bar.H,
            L = bar.L,
            T = bar.T,
            N = bar.N
        };
    }
}