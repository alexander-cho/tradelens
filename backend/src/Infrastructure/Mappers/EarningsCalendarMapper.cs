using Core.Models;
using Infrastructure.Clients.Fmp.DTOs;

namespace Infrastructure.Mappers;

public static class EarningsCalendarMapper
{
    public static EarningsCalendarModel ToEarningsCalendarDomainModel(EarningsCalendarDto earningsCalendarDto)
    {
        return new EarningsCalendarModel
        {
            Symbol = earningsCalendarDto.Symbol,
            Date = earningsCalendarDto.Date,
            EpsActual = earningsCalendarDto.EpsActual,
            EpsEstimated = earningsCalendarDto.EpsEstimated,
            RevenueActual = earningsCalendarDto.RevenueActual,
            RevenueEstimated = earningsCalendarDto.RevenueEstimated
        };
    }
}