# Time is money

I could probably count on one hand the number of things I was satisfied with my original implementation of the options chain
total cash values per strike and max pain calculation workflow. That's not downplaying the impact of the service nor my
abilities as I see few, if any free services that offer something similar in the manner that I envision as a smooth
user experience regarding point-in-time options flow data.


### Initial facelift

The process with my Flask prototype involved navigating to the `/symbol` path, searching for a ticker symbol and navigating
to its respective page if it exists, then clicking on the green Option Chain button to see the expiry calendar. Then
select an expiration date, and finally the user is given the option to view cash values and max pain. Any user realizes
that things can take quite some time among the last two of the aforementioned steps.

<img alt="SOFI max pain network req 2026-01-30" height="191" src="images/sofi-jan-30-26.png" width="312"/>
<img alt="AAPL max pain network req 2026-01-09" height="191" src="images/aapl-jan-9-26.png" width="312"/>
<img alt="QQQ max pain network req 2026-01-05" height="191" src="images/qqq-jan-5-26.png" width="312"/>

As seen above, most responses took between an entire second or two, with the tickers with more strike prices for that 
expiry being on the higher end of that as they are more computationally intensive. This is unacceptable for a few reasons
other than the fact that single chart renders take so long, no dynamic functionality, and ability to easily 
navigate between different expiry's and ticker symbols on demand.

Currently, what we have set up is a client layer that is concerned with making the request to the third party API,
specifically the Tradier `Get Options Chain` endpoint at `https://api.tradier.com/v1/markets/options/chains`. This is
handled by `apps/tradelens/src/Tradelens.Infrastructure/Clients/Tradier/TradierClient.cs`, where `GetOptionsChainAsync`
takes in a parameter object consisting of the symbol, expiration date, and greeks. Then we accomodate the shape of the
JSON response with a custom DTO. The `GetOptionsChainAsync` method in `apps/tradelens/src/Tradelens.Infrastructure/Services/OptionsService.cs`
calls the client layer method injected through the constructor, which is then called in the Api Controller method. The
middle service layer is responsible for taking that response shape from the Tradier API and performing the max pain
calculation.

I also noticed that in `OptionsService.cs`, there are two methods that do very similar things, i.e. they both call the
`GetOptionsChainAsync` method from `TradierClient.cs`. I will consider merging these two and creating a combined response
domain model instead, which we will come back to later in this doc.


### User Flow

When a user navigates to `/options`, they are able to type in and enter a ticker symbol and then a dropdown with the list
of expiration dates is available to select from. Once the user selects an expiration date, a bar chart containing the
total cash values for both calls and puts, as well as the max pain strike appears dynamically. When the user clicks the
expirations dropdown and selects another date, the corresponding chart will render. Logically, and by specific requests,
users will naturally look to see charts and data for numerous expiration dates for various reasons including but not limited to:
predicting where a stock price might be headed short term, where it may close by the end of the week, look at the general
flow for that day, etc. Especially if they have open position(s) in their portfolio, they will seek any data regarding
those, and look for other opportunities as well. They will also go back and look at the same chart(s) within a short 
period of time for comparisons, confirmations, etc.

In these scenarios it would be wasteful to call the Tradier API options chain endpoint with the same parameters 
multiple times within a short period. The amount of data received, including the max pain calculations is a non-trivial
task so it would be nice if we could use some mechanism to have quicker access to frequently requested data, and without 
hitting Tradier API again.

##### Consider for potential

Let's say we want to have STACKED options charts, i.e. have bars for multiple expiration dates (when comparing volume or
open interest across strike prices), then we need to call the API endpoint x amount of times, and it would be very helpful
to cache those calls so if the user enters a different combination of expiration dates, only the ones that don't exist
in the cache need to be retrieved from Tradier (or any other real-time options data provider).


### Solution

Redis, short for Remote Dictionary Server is a single-threaded, in-memory data structure server. Although more threads are
used in the background for data persistence and invalidation, only one is used for command execution which helps avoid
race conditions. Any data set by Redis is stored in RAM and not the disk which makes it extremely fast. Imagine you are 
at a library and someone comes up to you and asks how many books are on that shelf. You head over and count them and find
out there are 40. If another person comes and asks you the same a minute later, you won't have to go back and count them one-by-one.
However, that data can be lost more easily, so there are trade-offs as it relates to data durability. We can also make use
of common data structures like lists, strings, sets, hashes that are supported natively, enabling actions that they support
such as adding/removing/incrementing, etc.

To take advantage of this functionality, we will get an instance of Redis running locally with Docker, and consider using
a managed service from a Cloud provider in production later.

```shell
docker run -d \
--name tradelens-redis-dev \
-p 6380:6379 \
redis:latest
```

To make a connection, install the StackExchange.Redis package into the Tradelens.Infrastructure project as this is a 
data access concern.

In `apps/tradelens/src/Tradelens.Api/Extensions/CachingExtensions.cs`, we add an extension method to create an instance of
the `CollectionMultiplexer` class, get the connection string from app settings through IConfiguration, with an `AddSingleton` 
lifetime. we want to be able to access this same Redis instance for the lifetime of the application once it starts; the
official docs from the source code suggests the same `StackExchange.Redis/docs/Basics.md`.

```csharp
public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration config)
{
    services
        .AddSingleton<IConnectionMultiplexer>(_ =>
        {
            var connectionString = config.GetConnectionString("Redis");
            if (connectionString == null)
            {
                throw new Exception("Cannot get Redis connection string");
            }

            var configuration = ConfigurationOptions.Parse(connectionString, ignoreUnknown: true);
            return ConnectionMultiplexer.Connect(configuration);
        });

    return services;
}
```

Now it's time to create the service that will allow us to implement the caching feature. Create an interface in the Core
project that supports caching the response, getting, and removing it. First, the `Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);`
takes 3 parameters, providing us a key to query the cached response, the response itself cast as type object, and a TTL.
`Task<string?> GetCachedResponseAsync(string cacheKey);` searches for the cache key in the database, and returns the cached
response as a string, having been converted from object. We will explore creating the unique cache key later on. For now, 
let's create a service to implement the interface, injecting IConnectionMultiplexer that we registered as a service in 
the DI container earlier. Add this line as a class member to obtain an interactive connection to a database inside Redis.

```csharp
private readonly IDatabase _database = redis.GetDatabase(0);
```

Implement `CacheResponseAsync()`:

```csharp
public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
{
    var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    var serializedResponse = JsonSerializer.Serialize(response, options);
    await _database.StringSetAsync(cacheKey, serializedResponse, timeToLive);
}
```

According to the Microsoft docs, the web default policy for JSON is camel case, but we include the first line 
just to be sure. Here we are concerned with converting the response object into a string, and setting it.

Implement `GetCachedResponseAsync()`:

```csharp
public async Task<string?> GetCachedResponseAsync(string cacheKey)
{
    var cachedResponse = await _database.StringGetAsync(cacheKey);

    if (cachedResponse.IsNullOrEmpty)
    {
        return null;
    }

    return cachedResponse;
}
```

This is responsible for looking up a response in the database by the cacheKey. If it exists, return the cached response,
otherwise it will make an API request, DB call, etc. Now we can add the service to the DI container to use throughout the
app, once again as a Singleton, we want different users to be able to access the same redis instance. Add this to `AddRedis()`
in the extension method from earlier.

```csharp
.AddSingleton<IResponseCacheService, ResponseCacheService>()
```

Now it's ready for use, we can inject the service through the constructor of any class and use the methods we created above.

```csharp
//....
    private readonly ICompanyFundamentalsService _companyFundamentalsService;
    private readonly IResponseCacheService _cache;
    
    public CompaniesController(ICompanyFundamentalsService companyFundamentalsService, IResponseCacheService cache)
    {
        _companyFundamentalsService = companyFundamentalsService;
        _cache = cache;
    }
    
    [HttpGet("company-profile")]
    public async Task<ActionResult<CompanyProfile>> GetCompanyProfile([FromQuery] string ticker)
    {
        // implement some logic to create a cache key out of the url/request, or anything uniquely identiable depending on use case
        // if GetCachedResponseAsync(cacheKey) with this specific cache key returns something, use that as the response
        // if not hit the companyFundamentalsService (DB, third party API, file, etc.), then cache that response, return it
        return Ok(await _companyFundamentalsService.GetCompanyProfileDataAsync(ticker));
    }
//....
```

This is not an optimal design for a few reasons. First, we'd have to write a lot of repetitive code, likely every class 
where we have a method to cache the response of, and within each method as well. Also, we should keep our controllers 
as thin as possible, only concerning ourselves with the request and response, and infrastructure concerns like caching 
away from it. 

#### Trade-offs between data freshness and longer TTL
