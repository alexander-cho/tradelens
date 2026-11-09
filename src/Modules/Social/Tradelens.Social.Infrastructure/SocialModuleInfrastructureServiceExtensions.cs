using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Tradelens.Social.Domain.Posts;
using Tradelens.Social.Infrastructure.Posts;

namespace Tradelens.Social.Infrastructure;

public static class SocialModuleInfrastructureServiceExtensions
{
    public static IServiceCollection AddSocialModuleInfrastructure(this IServiceCollection services,
        IConfiguration config)
    {
        return services
            .AddScoped<IPostRepository, PostRepository>()
            .AddDatabase(config);
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<SocialModuleSettings>(
            config.GetSection(SocialModuleSettings.SectionName));

        services.AddDbContext<SocialDbContext>((serviceProvider, opt) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<SocialModuleSettings>>().Value;
            opt.UseNpgsql(settings.DefaultConnection);
        });

        return services;
    }
}
