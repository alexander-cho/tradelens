# Repository pattern - Data access v.s. Domain-centric

### Abstracting away EF Core with repositories

Each entity in the Core gets its own repository, whether explicit, or through the Generic Repository, which supports
common methods and operations. When we need more custom logic, use the Specification pattern to extend and handle more
complex queries without having to create custom repositories. If we want to create a dropdown in the UI
where users can filter by a specific Entity attribute, get a list of the unique instances of that which is a list of strings, 
not an Entity.

```csharp
public interface IGenericRepository<T>
{
    IReadOnlyList<T> FindAsync(Expression<Func<T, bool>> query);
}
```

The above is a leaky abstraction, exposing details that should be encapsulated. It also assumes that we are using a LINQ 
provider to access data. End users of the API can query anything with this, including `await productRepo.FindAsync(x => x.Price < 0);`
which is nonsensical. Also, this causes business logic to potentially be scattered, i.e. two devs can query the same thing 
differently to get "active" entities as an example:

```csharp
await repo.FindAsync(x => x.IsActive);
await repo.FindAsync(x => x.IsActive && x.Count > 0);
```

Instead, we can limit access by defining a set of rules in the core domain layer, through specification objects.
At least if we use `System.Linq.Expressions` in the Core layer, e.g. `Tradelens.Core/Specifications/BaseSpecification.cs`,
the expression tree evaluation would be "controlled" in a sense and encapsulated via a concrete specification class.
By using specifications for complex/non-generic logic, we can expose a clean API that hides internal implementation details,
query logic, and business rules.

### Using a domain driven approach

For some business rules we can establish clear boundaries between entities. For example, for a blog feed functionality,
we will have Posts with Comments and Likes, etc. associated with it. Any rules such as: on deletion of a post we must
delete the associated Comments and Likes, or a User cannot add a Like to their own Post, etc. are closely related. Then,
our application truly revolves around the rich domain logic, as opposed to a Generic Repository which, really, is a wrapper
around database operations. We could have domain methods such as GetLikesAssociatedWithPost(), which would work regardless
of if the Infrastructure layer implements EF Core, stored procedures, a CSV file, an external API, etc.
