using Tradelens.Core.Entities;
using Tradelens.Infrastructure.Data;

namespace Tradelens.Api.Extensions;

public static class IdentityExtensions
{
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    { 
        services.AddAuthorization();
        services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<TradelensDbContext>();

        return services;
    }
}