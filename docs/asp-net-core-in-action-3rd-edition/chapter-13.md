## 13

ASP.NET Core MVC follows the MVC pattern, uses *Controllers* containing *Action methods* over Razor pages and page handlers. Razor pages uses ASP.NET Core MVC under the hood for its behavior.

*Binding model* is one or more objects that acts as a container for data provided in a request, required by the page handler. Conceptually equivalent to parameters passed from minimal API endpoint populated from the request.

## 14

Routing styles => explicit routes where each endpoint or controller class defines its own route pattern (minimal api and Web Api MVC)

## ..

razor pages

## 16
Model binding and validation; revisit

## 19
**In legacy ASP.NET** \
MVC Controllers => controller classes used with the MVC framework to generate and return HTML \
Web API Controllers => controller classes used with the Web API framework to generate JSON or XML

**In modern ASP.NET Core**
The two merged into ASP.NET Core MVC framework which can generate all HTML, JSON, XML, but usually controllers tend to dedicate to one return type for consistency, so "MVC controller => HTML" and "Web API controller => JSON/XML" distinction is still used.