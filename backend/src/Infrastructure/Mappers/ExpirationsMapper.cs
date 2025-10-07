using Core.Models;
using Infrastructure.Clients.Tradier.DTOs;

namespace Infrastructure.Mappers;

public static class ExpirationsMapper
{
    public static ExpirationsModel ToExpirationsDomainModel(ExpirationsDto expirationsDto)
    {
        return new ExpirationsModel
        {
            Expirations = ToFullExpiryListDomainModel(expirationsDto.Expirations)
        };
    }

    private static Core.Models.FullExpiryList ToFullExpiryListDomainModel(Clients.Tradier.DTOs.FullExpiryList fullExpiryList)
    {
        return new Core.Models.FullExpiryList
        {
            Date = fullExpiryList.Date
        };
    }
}