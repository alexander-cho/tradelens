namespace Tradelens.Social.Infrastructure;

public sealed class SocialModuleSettings
{
    public const string SectionName = "Modules:Social";

    public string DefaultConnection { get; set; } = string.Empty;
}
