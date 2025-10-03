using Core.Models;
using Infrastructure.Clients.Tradier.DTOs;

namespace Infrastructure.Mappers;

public static class OptionsChainMapper
{
    public static OptionsChain ToOptionsChainDomainModel(OptionsChainDto optionsChainDto)
    {
        return new OptionsChain
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
            OpenInterest = strikePriceData.OpenInterest,
            OptionType = strikePriceData.OptionType,
            Strike = strikePriceData.Strike,
            Symbol = strikePriceData.Symbol,
            Underlying = strikePriceData.Underlying
        };
    }
}