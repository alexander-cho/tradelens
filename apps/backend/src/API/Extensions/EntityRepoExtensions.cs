using Core.Interfaces;
using Infrastructure.Repositories;

namespace API.Extensions;

public static class EntityRepoExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            // type of entity to be used with generic repositories is unknown at this point- typeof()
            .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddScoped<IPostRepository, PostRepository>();

        return services;
    }
}