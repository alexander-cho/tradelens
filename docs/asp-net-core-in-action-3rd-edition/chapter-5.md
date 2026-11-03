Middleware pipeline for general cross-cutting concerns

Filters for endpoint specific cross-cutting concerns

Minimal Api's -> `.AddEndpointFilter()` fluent syntax inside method endpoint body

Controller Action filter -> `Attribute` class, `IActionFilter`, etc.

Similarities of Middleware vs Filter pipeline
- request passing through and again response pass through on the way out
- can short circuit and return response instead of passing it further down

Differences:
- Middleware runs for all requests coming into the application, filters only when the request reaches the endpoint middleware
- Filters have access to additional details about the endpoint or action to be executed such as its return value
- Filters are used more if you want to restrict behavior to certain controller classes or methods (endpoints) such as response caching, finer-grained authorization, etc.