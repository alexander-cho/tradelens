using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Tradelens.Social.Application.Posts;
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
            .AddScoped<IPostReadRepository, PostReadRepository>()
            .AddEfCoreConnection(config)
            .AddDapperConnection(config);
    }

    private static IServiceCollection AddEfCoreConnection(this IServiceCollection services, IConfiguration config)
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

    private static IServiceCollection AddDapperConnection(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<SocialModuleSettings>(
            config.GetSection(SocialModuleSettings.SectionName));
        
        services.AddScoped<IDbConnection>(serviceProvider =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<SocialModuleSettings>>().Value;
            var conn = new NpgsqlConnection(settings.DefaultConnection);
            conn.Open();
            return conn;
        });

        return services;
    }
}
