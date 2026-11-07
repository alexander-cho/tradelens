## Configuration

We can load app settings and secrets (key-value pairs) from multiple files or locations (Configuration providers), json, xml, yaml; or database or environment variables, cli args, etc.

`ConfigurationManager` class introduced in .NET 6 simplifies common patterns for working with configuration, which `WebApplicationBuilder` exposes with the property `Configuration`.

Using multiple configuration providers, the order matters where later keys with the same name override those in providers that were registered earlier.

During development, use appsettings.{Environment}.json or User Secrets manager, latter being safer since we will not accidentally push to source control.

**Prefer strongly typed settings**: when retrieving and using keys
Use the `IOptions<>` wrapper interface to inject a strongly typed options class.
Options classes need to be nonabstract and have a parameterless public constructor to be eligible for binding of the configuration values to the POCO's. 
```csharp
public class SocialModuleSettings
{
    public string DefaultConnection { get; set; }
    public string SomeApiKey { get; set; }
}

// then configure options classes as services in the DI container
builder.Services.Configure<SocialModuleSettings>(
    builder.Configuration.GetSection("SocialModuleSettings"));
```

We can implement complex types for settings as well and not just primitives like strings.

ASP.NET Core introduces the `IOptions<T>` interface which has a single property `Value` that contains the configured POCO at runtime.

### Different environments

The `WebApplicationBuilder` instance uses environment variable `ASPNETCORE_ENVIRONMENT` to determine the environment its running on, creating an `IHostEnvironment` object that's exposed as `WebApplicationBuilder.Environment`.

Three common extension methods: `IHostEnvironment.IsDevelopment()`, `IHostEnvironment.IsStaging()`, `IHostEnvironment.IsProduction()`.

