Web Api controller action methods return JSON by default

Using `ActionResult<T>` vs `IActionResult` for return type, using the former lets us return an instance of the type `T` *or* an implementation of `ActionResult` such as `NotFoundResult`.

The request reaches the WebApi controller Action method, which invokes application/domain logic/models to create the data necessary to service the request and returns it back to the Action method, which then creates an `API model`. The `output formatter` which is used in conjunction, takes that API model and serializes it in the appropriate machine-readable format to send back as response, e.g. JSON, XML, etc.

When returning an error response, returning the details of the issue with the `ProblemDetails` class is recommended. Decorating controller classes with the `[ApiController]` attribute gives us many things out of the box such as attribute routing, converting error status codes returned by controller and converting to ProblemDetails responses, etc.

**Content negotation (conneg)**: where a client says which types of data it can accept using the `Accept` header in the request.