using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tradelens.Social.Infrastructure;
using Tradelens.Social.Presentation;

namespace Tradelens.Social.Startup;

public static class SocialModuleExtensions
{
    public static IServiceCollection AddSocialModule(this IServiceCollection services, IConfiguration config)
    {
        services.AddSocialModuleInfrastructure(config);

        services.AddSocialModulePresentation();

        return services;
    }
}