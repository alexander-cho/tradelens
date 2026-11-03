DI container usage => a class that depends on another class or service does not need to be responsible for creating an instance of it or knowing how to create it

**keep code loosely coupled by programming to interfaces**
- for example, a consuming `RegisterUserCommand` can depend on an `IEmailSender` instead of `EmailSender`, having the DI container select the implementation at runtime dependent on how we configure it (registration).

MVC controllers request dependencies from the DI container explicitly via constructor.

DI is central to ASP.NET Core, internal components such as Kestrel are configured with it, as well as extra and defined features with the `WebApplicationBuilder.Services (IServiceCollection)`.

<for interface X, use implementation Y>

The DI container creates a dependency graph, which is the set of objects that need to be instantiated to create the requested "root" caller object. When the DI container creates the concrete implementation registered, it creates all of that service's dependencies and uses it to create the final instance.

Two ways to access registered services from the DI container
- constructor injection (default to this)
- service locator pattern (calling the DI container directly) (`WebApplicationBuilder.Services (IServiceProvider)`)
    - `IServiceProvider` acts as a service locator, so use methods like `GetRequiredService<T>()`.

The service locator pattern should be avoided in favor of direct constructor injection
- Dependency Injection Principles, Practices, and Patterns, by Steven van Deursen and Mark Seemann (Manning, 2019) goes into more detail