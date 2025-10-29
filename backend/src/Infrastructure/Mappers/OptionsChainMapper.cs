using Core.Models;
using Infrastructure.Clients.Tradier.DTOs;

namespace Infrastructure.Mappers;

public static class OptionsChainMapper
{
    public static OptionsChainModel ToOptionsChainDomainModel(OptionsChainDto optionsChainDto)
    {
        return new OptionsChainModel
        {
            OptionsChain = optionsChainDto.Options.Option
                .Select(ToStrikePriceDataModel)
                .ToList()
        };
    }

    private static Core.Models.StrikePriceData ToStrikePriceDataModel(Clients.Tradier.DTOs.StrikePriceData strikePriceData)
    {
        return new Core.Models.StrikePriceData
        {
            Description = strikePriceData.Description,
            ExpirationDate = strikePriceData.ExpirationDate,
            Volume = strikePriceData.Volume,
            OpenInterest = strikePriceData.OpenInterest,
            Last = strikePriceData.Last,
            Bid = strikePriceData.Bid,
            Ask = strikePriceData.Ask,
            OptionType = strikePriceData.OptionType,
            Strike = strikePriceData.Strike,
            Symbol = strikePriceData.Symbol,
            Underlying = strikePriceData.Underlying,
            Greeks = ToGreeksModel(strikePriceData.Greeks),
            ImpliedVolatility = ToImpliedVolatilityModel(strikePriceData.Greeks),
            Activity = ToActivity(strikePriceData)
        };
    }

    private static Core.Models.Greeks ToGreeksModel(Clients.Tradier.DTOs.Greeks? greeks)
    {
        if (greeks != null)
            
        {
            return new Core.Models.Greeks
            {
                Delta = greeks.Delta,
                Gamma = greeks.Gamma,
                Theta = greeks.Theta,
                Vega = greeks.Vega,
                Rho = greeks.Rho,
                Phi = greeks.Phi,
                UpdatedAt = greeks.UpdatedAt
            };
        }

        return new Core.Models.Greeks();
    }

    private static Core.Models.ImpliedVolatility ToImpliedVolatilityModel(Clients.Tradier.DTOs.Greeks? greeks)
    {
        if (greeks != null)
        {
            return new Core.Models.ImpliedVolatility
            {
                BidIv = greeks.BidIv,
                MidIv = greeks.MidIv,
                AskIv = greeks.AskIv,
                SmvVol = greeks.SmvVol,
                UpdatedAt = greeks.UpdatedAt
            };
        }
        return new Core.Models.ImpliedVolatility();
    }

    private static Core.Models.Activity ToActivity(Clients.Tradier.DTOs.StrikePriceData strikePriceData)
    {
        return new Activity
        {
            OpenInterest = strikePriceData.OpenInterest,
            Volume = strikePriceData.Volume
        };
    }
}