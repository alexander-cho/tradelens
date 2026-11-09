using Microsoft.Extensions.DependencyInjection;

namespace Tradelens.Social.Presentation;

public static class SocialModulePresentationServiceExtensions
{
    public static IServiceCollection AddSocialModulePresentation(this IServiceCollection services)
    {
        // https://learn.microsoft.com/en-us/aspnet/core/mvc/advanced/app-parts?view=aspnetcore-10.0
        services.AddControllers().AddApplicationPart(typeof(SocialModulePresentationServiceExtensions).Assembly);

        return services;
    }
}