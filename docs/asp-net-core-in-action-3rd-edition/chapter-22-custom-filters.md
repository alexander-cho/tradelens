## Custom filters

We can consider creating and using filters when a piece of logic is orthogonal to the intent of an action method, which is receiving a request and returning a response.

To short-circuit a pipeline, we set the `context.Result` property to an `IActionResult` instance. The framework will execute this result to generate the response, and any remaining filters in the pipeline will be bypassed and the action method is also skipped. This applies to most filters, other than cases such as action filter short-circuiting where the result filter still runs after.

### Authorization filters
ASP.NET Core has a built-in authorization framework, which at the center is the `AuthorizeFilter` in which we decorate class or action methods with `[Authorize]`. At its base, this filter checks if an *authenticated* user is making the request, otherwise the pipeline short-circuits and return a 401 to the browser.

### Resource filters
They are the first general purpose filters, and are very versatile due to the fact that they execute so early and late in the pipeline. An example of a built-in resource filter is the `ConsumesAttribute` which if we decorate a controller class or action method with `Consumes("application/json")`, then if the request is sent in XML, the resource filter will short-circuit the pipeline and return a 415.

They are good for when we want something to run early on in the pipeline, and before model binding.

Used for feature toggle (feature flags) when we want to disable the respective part of the API based on a condition. Matching my preconcieved notion, the author admits this can be done using authorization filters but recommends not writing our own authorization filters.

### Action filters
Run right after model binding and before action method execution. Due to this, they can access all the arguments that will be used to execute the action method such as the request route, parameters, headers, etc.

### Exception filters
We can handle an exception in the filter pipeline by generating an appropriate response body for API clients. The exception handler middleware only intercepts errors without a body, so it will let the modified web API response pass back up untouched.

Exception filters can catch exceptions from action methods as well as during model binding and validation, during action method execution, and during action filter execution.

They don't catch any exceptions thrown in filters other than action filters, make sure resource/result filters, etc. don't throw exceptions.

Exception filters are usually not necessary if we handle everything in exception middleware, it is used in cases such as where we use both Razor Pages and MVC in the same host, and we cannot return HTML to a client that sent the request with Accept application/json for example.

### Result filters
Assuming everything in the pipeline so far has run fine without exception handling or short-circuiting, immediately after action filters are result filters. They run before and after the `IActionResult` returned by the action method (or action filters) is executed.

*If the pipeline is short-circuited before by setting `context.Result`, the result filter stage will not run but the `IActionResult` will still be executed to generate the response. Exceptions to this rule are action and page filters (analogous), which only short-circuit the action execution, and result filters run as normal.*

Custom result filters tend to be rare in the author's experience. They are usually used to customize the way `IActionResult` executes. ASP.NET Core has some built-in result filters such as `ProducesAttribute` forces the web API to serialize the result in a specific output format such as json or XML, etc. not regarding what the client can accept.

___

Filters should be more of a last-resort tool to extract repetitive and HTTP-related or cross-cutting code from controllers. Otherwise it can be preferrable to write a private method inside the controller class or push functionality into the domain.

### DI with filter attributes
We cannot pass in dependencies into attributes' constructors other than constant values, and attributes are created as singletons.

In my own `CacheAttribute` in `src/_legacy/Tradelens.Api/RequestHelpers/CacheAttribute.cs` I obtain an instance of `IResponseCacheService` (which is also a singleton) using the service locator pattern. The attribute currently extends the `Attribute` class and implements `IAsyncActionFilter` interface and the key here is the split the two, i.e. create a filter class that contains the functionality and an attribute that tells the framework (DI container) when and where to use the filter.

In the case of this app, `CacheFilter` contains the functionality, and can be used as a global filter if desired. Then `CacheAttribute` will extend `TypeFilterAttribute` which impelments `IFilterFactory`, and we pass the `Type` of filter to create as an argument to the `base` constructor.

Now when an action method decorated with `CacheAttribute({ttl})` is called, the framework calls the `CreateInstance()` method on the `IFilterFactory` attribute, creating a new instance of the `CacheAttribute` and uses the DI container to populate its dependencies, in this case the `IResponseCacheService`.

Another `IFilterFactory` implementation `ServiceFilterAttribute` loads the filter itself from the DI container, but we need to explicitly register the filter with the lifetime with the DI container.

*The author prefers to write the filter as a nested class inside of the attribute class for organization.*