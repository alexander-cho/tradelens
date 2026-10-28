using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tradelens.Social.Application.Posts;
using Tradelens.Social.Infrastructure.Posts;

namespace Tradelens.Social.Infrastructure;

public static class PostsModuleInfraServiceExtensions
{
    public static IServiceCollection AddPostsModuleInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        return services
            .AddScoped<IPostRepository, PostRepository>()
            .AddDatabase(config);
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<SocialDbContext>(opt =>
        {
            opt.UseNpgsql(config.GetConnectionString(""));
        });
        return services;
    }
}