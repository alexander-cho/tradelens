using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tradelens.Social.Infrastructure;

namespace Tradelens.Social.Startup;

public static class SocialModuleExtensions
{
    public static IServiceCollection AddSocialModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddSocialModuleInfrastructure(config);

        return services;
    }
}
