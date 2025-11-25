using Core.Models;
using Infrastructure.Clients.Fred.DTOs;

namespace Infrastructure.Mappers;

public static class FredMappers
{
    
}

public static class MarginBalanceMapper
{
    public static MarginBalanceModel ToMarginBalanceModel(MarginBalanceDto marginLoansDto)
    {
        return new MarginBalanceModel
        {
            ObservationStart = marginLoansDto.ObservationStart,
            ObservationEnd = marginLoansDto.ObservationEnd,
            Units = marginLoansDto.Units,
            Count = marginLoansDto.Count,
            Observations = marginLoansDto.Observations.Select(DataPointMapper.ToDataPointModel).ToList()
        };
    }
}

public static class MoneyMarketFundsMapper
{
    public static MoneyMarketFundsModel ToMoneyMarketFundsModel(MoneyMarketFundsDto moneyMarketFundsDto)
    {
        return new MoneyMarketFundsModel
        {
            ObservationStart = moneyMarketFundsDto.ObservationStart,
            ObservationEnd = moneyMarketFundsDto.ObservationEnd,
            Units = moneyMarketFundsDto.Units,
            Count = moneyMarketFundsDto.Count,
            Observations = moneyMarketFundsDto.Observations.Select(DataPointMapper.ToDataPointModel).ToList()
        };
    }
}

internal static class DataPointMapper
{
    internal static DataPointModel ToDataPointModel(DataPoint dataPoint)
    {
        return new DataPointModel
        {
            Date = dataPoint.Date,
            Value = ParseValue(dataPoint.Value),
            RealtimeStart = dataPoint.RealtimeStart,
            RealtimeEnd = dataPoint.RealtimeEnd
        };
    }

    private static decimal? ParseValue(string value)
    {
        // FRED returns "." for missing values
        if (string.IsNullOrWhiteSpace(value) || value == ".")
        {
            return null;
        }

        return decimal.TryParse(value, out var result) ? result : null;
    }
}