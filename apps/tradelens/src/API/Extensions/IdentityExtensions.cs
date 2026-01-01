using Core.Entities;
using Infrastructure.Data;

namespace API.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    { 
        services.AddAuthorization();
        services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<TradelensDbContext>();

        return services;
    }
}