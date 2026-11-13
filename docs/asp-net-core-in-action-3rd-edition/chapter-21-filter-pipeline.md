## Filter pipeline

Like middleware, filters are good for handling cross-cutting concerns in the application as well as reducing duplicated code in action methods. Some differences however are that middleware can run for all requests while filters only if it gets to the endpoint middleware. Filters can be applied to a subset of requests with attribute decoration. Filters are also for MVC constructs such as `IActionResult`.

**Authorization filters** run first and only once throughout the pipeline, if the request is unauthorized it is short-circuited and the rest of the filter pipeline and action do not execute.

**Resource filters** run second, and they can also run at the end of the pipeline as well. It can also short-circuit the request as well if for example an unsupported content type is requested. We can add metrics to the request, or control the way model-binding (next step in the pipeline) works for the request.

*Model binding*: ASP.NET Core binds the request data to the action method's parameters.

**Action filters** are third, run immediately before and after an action method is executed, and can modify the arguments to the method before it executes, since it happens after model-binding occurs. Alternatively it can also short-circuit the request and send an entirely different `IActionResult`. Once the method runs and an `IActionResult` such as `Ok()` or `NotFound()` is returned, the filter runs again, and can customize that returned `IActionResult` once more before MVC executes the action result.

**Exception filters** catch exceptions that occur in the filter pipeline and handle them, runs only once throughout the pipeline.

**Result filters** run last, before and after an `IActionResult` is executed. They are used to control the execution of the result or short-circuit the execution.

Some use middleware components inside MVC filters which is possible.

Each execution of a filter executes a different method of the appropriate interface, e.g. for action filters, we extend the `Attribute` class and implement `IAsyncActionFilter` which has the method `OnActionExecutionAsync()`.

```csharp
public class SampleActionAttribute() : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // Called before the rest of the pipeline executes
        Console.WriteLine("Executing async");

        // Executes the rest of the pipeline and obtains an ActionExecutedContext
        ActionExecutedContext executedContext = await next();

        // Called after the rest of the pipeline executes
        Console.WriteLine("Executed async");
    }
}
```

The *scope* of a filter refers to how many different actions it applies to, i.e. action (decorate method), controller (decorate class), global (explicitly add to DI container by overloading `builder.Services.AddControllers()` with `MvcOptions`).

### Filter execution order

In terms of scopes, globally scoped filters run first, then controller (derived), then base controller, then action method last (broadest -> narrowest).

For more fine-grained filter execution order scenarios such as having multiple Action filters on the same scope, override the default order by implementing `IOrderedFilter`, which contains a single property `Order` of type int which is 0 by default.