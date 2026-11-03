## DI, service registration

`builder.Services.Add*<TService, TImplementation>();`

```csharp
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped(
    provider =>
    new EmailServerSettings(
        Host: "smtp.server.com",
        Port: 25
    )
);

public record EmailServerSettings(string Host, int Port);

public interface IEmailSender
{
    void Send(string emailAddress, string subject, string content);
}

public class EmailSender : IEmailSender
{
    public EmailSender() {}

    public async void Send(string emailAddress, string subject, string content)
    {
        // impl
    }
}
```

When using `Add*<TService, TImplementation>, which use generics to specify the Type of class to register, the built-in DI container makes assumptions we must adhere to:
- The class must be a concrete type
- The class must have only a single relevant constructor that the container can use
- For a constructor to be relevant, all constructor arguments must be registered with the container or must be arguments with a default value

A record like `EmailServerSettings()` has primitive type dependencies such as `string` and `int` which don't make sense to register in the container, i.e. "everytime a `string` type is requested, use `"smtp.server.com"`. Instead, we can provide a pre-constructed `EmailServerSettings` object to the container so it can use that whenever needed.

For scoped lifetimes as opposed to singletons, using a lambda in the example above, `provider` is an instance of `IServiceProvider`, and is the public API of the DI container itself, exposing GetRequiredService<T>(). This way we discover dependencies at runtime, but calling these extension method(s) in factory functions should be avoided if possible in favor of constructor injection.

### Service lifetimes

**Transient**: everytime the dependency is required, a new one is created
- like calling new() everytime a new instance is needed
- lightweight services with little to no state

**Scoped**: same instance of object will be used within a given scope
- in ASP.NET Core, a scope maps to an HTTP request
- many ASP.NET Core and related services are scoped due to the nature of web requests (Database contexts, Authentication)
    - A write command to the data store using the parameters, headers, etc. from a POST web request to perform a transaction. Subsequent request will have different data attached- different context(s)
    
**Singleton**: created once when needed or during registration, and services get the same instance injected
- for services that are expensive to create, has data that needs to be shared across multiple requests, or don't hold state.
- they have to be thread-safe since multiple threads can use the same instance during concurrent requests

### Pitfalls

**Captive dependency**: Dependency with an incorrectly configured lifetime, e.g. a service with a longer lifetime depends on one with a shorter one.

```csharp
builder.Services.AddSingleton<IRepository, Repository>();
builder.Services.AddScoped<IDataContext, DataContext>();

public class SomeCommand
{
    private readonly IRepository _repository;
    public SomeCommand(IRepository repository)
    {
        _repository = repository;
    }
    // ...
}

public class Repository : IRepository
{
    private readonly IDataContext _dataContext;
    public Repository(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }
    // ...
}

public class DataContext : IDataContext {}
```

With the above example, the first request coming in calling `SomeCommand` will create instances of `Repository` and `DataContext` as registered in the DI container, the former as a singleton and latter as scoped. When a second request comes in, the same `Repository` instance created before is used, which has a dependency to scoped `DataContext`, which is now also hanging around as a singleton.

Solution: Services should only depend on other services with a longer or equal lifetime then itself.

Resolving dependencies directly from `WebApplication.Services` with `GetRequiredService<T>()` instead of constructor injection works for singletons since the `Services` property from `WebApplication` exposes the root DI container as `IServiceProvider` and are only disposed of when the app shuts down. To safely resolve a scoped service we must create an instance of `IServiceScope` by calling `CreateScope()`. `IServiceScope` also exposes the `IServiceProvider` root DI container but now we can resolve scoped and transient services. `IServiceScope` implements `IDisposable`.

*from `tests/_legacy/Tradelens.Api.IntegrationTests/TradelensWebApplicationFactory.cs`*
```csharp
Db = Services.CreateScope().ServiceProvider.GetRequiredService<TradelensDbContext>();
```

```csharp
await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
}
```

The author tells us to prefer `CreateAsyncScope()` over `CreateScope()`.