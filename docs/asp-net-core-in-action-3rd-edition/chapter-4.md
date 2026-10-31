## Middleware

In the context of ASP.NET Core, middleware is a C# class that handles an HTTP request or response. It's technically a function but middleware are often written in extension classes so we can attach them to the pipeline to execute sequentially. The pipeline is bidirectional, i.e. outgoing responses hit the same middleware components encountered during request processing but in reverse order.

- handle an incoming request and create a response out of it
- process incoming http request, modify it, pass it to next middleware 
- process outgoing http response, modify it, pass it to next (reverse) middleware or Kestrel

`EndpointMiddleware` class is responsible for generating the JSON response

Subsequent middleware can use details added by the previous middleware to do more to the request. For example, an Authentication middleware associates the incoming request with a registered user of the application, then a subsequent Authorization middleware determines if that specific user can access the requested resource, etc.

terminal middleware: middleware component that short-circuits the pipeline and returns a response

All middleware have access to the `HttpContext` object (initially constructed by Kestrel using the incoming raw request), which has all of the information about the request such as user credentials, etc. Kestrel converts the representation to a raw HTTP response and sends it to the reverse proxy and eventually client application.

If no middleware returns a response to a request, then ASP.NET Core automatically inserts a dummy middleware component that returns a 404 error to the client.

Error-handling middleware ignores the incoming request, and modifies the outgoing response if an error has occurred.

The nature of ASP.NET Core is opt-in, so we need to explicitly configure error handling in the application. When a user action ends in an exception such as `NullReferenceException`, it bubbles back up the middleware pipeline, and if no middleware component handles it, the web server returns a 500 status code.

Error handling middleware is often used to modify the format and/or content of the exception so it's more readable or detailed.

The middleware pipeline can be reexecuted as long as the response's headers have not been sent. For example if we have configured an Exception handler middleware (framework or custom) such as:

```csharp
...
app.UseExceptionHandler("/error");
app.MapGet("/error", () => "Sorry, an error occurred");
...
```

Then if a request to the path `"/"` throws an exception via somewhere in endpoint middleware, the exception propogates back up the pipeline until it hits the exception handler middleware (ignored on the way down as mentioned earlier) and it can reexecute the pipeline with the new error path `/error`, and generates the new response, and propogates again with a 500 and "Sorry, an error occurred".