Routing: mapping an incoming request to a method that will handle it (handler)
- in MVC, the handler is an action method in a controller

Routing middleware parses a route template e.g. `/products/{category}/{name}` by splitting into segments. `category` and `name` route parameters in this example.

Routing is also responsible for generating URL's so that other parts of the application can reference them for things like redirection, etc.