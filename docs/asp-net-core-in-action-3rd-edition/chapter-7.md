Model binding: taking the raw HTTP request (string(s)), extracts values from it and makes them available to use by the endpoint handlers as input by populating CLR objects (POCO).

If the name of the handler parameter matches the name of a route parameter in the route template, ASP.NET Core binds the handler parameter to it.

`/posts/{user}/{id}` => `[HttpGet("{user:string}/{postId:int}")] public async Task<ActionResult> GetPost(string user, int postId) {}`

if there is no match, it will try to bind to a query string value.

bind simple types such as `int`, `string`, etc. which internally implement `TryParse()`.

complex types, not implementing a `TryParse()`, such as if a controller action method has a parameter of 
`public record Post(int Id, string Content, DateTime CreatedAt)`, for example, sending a POST request, the body needs to contain a valid JSON, i.e. `{ "id": 1, "content": "Hello", "createdAt": 2025-12-27 08:26:49.219717 }`.

Query parameters don't have to be unique, for example user filters for products of two or more different categories: `/products?category=winter&category=summer`, this is represented in the method signature as an array, but work only if the HTTP verb does not contain a body, like a GET like the example, HEAD, DELETE, etc.