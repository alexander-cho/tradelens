using Core.Models;
using Infrastructure.Clients.Fmp.DTOs;

namespace Infrastructure.Mappers;

public static class CongressTradeMapper
{
    public static CongressTradeModel ToCongressTradesDomainModel(CongressTradeDto congressTradeDto)
    {
        return new CongressTradeModel
        {
            DisclosureDate = congressTradeDto.DisclosureDate,
            Amount = congressTradeDto.Amount,
            AssetDescription = congressTradeDto.AssetDescription,
            AssetType = congressTradeDto.AssetType,
            Link = congressTradeDto.Link,
            Office = congressTradeDto.Office,
            Owner = congressTradeDto.Owner,
            Symbol = congressTradeDto.Symbol,
            TransactionDate = congressTradeDto.TransactionDate,
            Type = congressTradeDto.Type
        };
    }
}