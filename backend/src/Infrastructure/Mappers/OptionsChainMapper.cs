using Core.Models;
using Infrastructure.Clients.Tradier.DTOs;

namespace Infrastructure.Mappers;

public static class OptionsChainMapper
{
    public static OptionsChainModel ToOptionsChainDomainModel(OptionsChainDto optionsChainDto)
    {
        return new OptionsChainModel
        {
            Options = ToFullOptionsChainDomainModel(optionsChainDto.Options)
        };
    }

    private static Core.Models.FullOptionsChain ToFullOptionsChainDomainModel(Clients.Tradier.DTOs.FullOptionsChain fullOptionsChain)
    {
        return new Core.Models.FullOptionsChain
        {
            Option = fullOptionsChain.Option.Select(ToStrikePriceDataDomainModel).ToList()
        };
    }

    private static Core.Models.StrikePriceData ToStrikePriceDataDomainModel(Clients.Tradier.DTOs.StrikePriceData strikePriceData)
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
            Greeks = ToGreeksModel(strikePriceData.Greeks)
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
                SmvVol = greeks.SmvVol,
                UpdatedAt = greeks.UpdatedAt
            };
        }

        return new Core.Models.Greeks();
    }
}