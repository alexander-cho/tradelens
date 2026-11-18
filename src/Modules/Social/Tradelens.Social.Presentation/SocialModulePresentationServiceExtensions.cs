using Microsoft.Extensions.DependencyInjection;
using Tradelens.Social.Application.Posts;

namespace Tradelens.Social.Presentation;

public static class SocialModulePresentationServiceExtensions
{
    public static IServiceCollection AddSocialModulePresentation(this IServiceCollection services)
    {
        // https://learn.microsoft.com/en-us/aspnet/core/mvc/advanced/app-parts?view=aspnetcore-10.0
        services.AddControllers().AddApplicationPart(typeof(SocialModulePresentationServiceExtensions).Assembly);
        return services.AddRequestHandlers();
    }

    // needs to be in application layer.
    private static IServiceCollection AddRequestHandlers(this IServiceCollection services)
    {
        return services
            .AddTransient<CreatePostCommandHandler>()
            .AddTransient<GetPostByIdQueryHandler>()
            .AddTransient<GetPostsQueryHandler>();
    }
}