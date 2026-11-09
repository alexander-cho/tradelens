## Entity Framework Core

EF Core especially code-first helps with speed of development, stay in familiar territory of writing object-oriented .NET, and maybe opt to do database-first for complex schemas or large apps. It handles database connection/pooling, mapping CLR objects -> database tables/columns, change tracking, etc. Hiding the database itself in this manner can also be a drawback as we defer more control to the library. It's also less performant than say raw SQL since EF Core (ORMs in general) are abstractions over several layers (which I presume the author is referring to Repository and Unit of Work, etc.).

A **migration** is a C# file in the application that describes how the data model changed, including tables, columns, relationships, etc.

### UPDATE
1. The update method receives a command indicating which entity to update and the new property values.
2. The DbContext generates the SQL necessary to load the entity from the Db (read). `await _dbContext.{DbSet<T>}.FindAsync(command.Id);`
3. The command is used to actually update the properties and relationships on that entity.
4. Save is called on the DbContext which generates necessary SQL to update the entity in the database.

EF Core internally internally tracks the state of any entities it loads from the database and creates a snapshot of its property values, so when we call `SaveChangesAsync()`, EF Core compares the state of any tracked entities with the tracking snapshot. The properties that have been changed are included in the UPDATE statement sent to the database while unchanged ones are ignored.

### DELETE
Some non-practical and difficult situation with deleting records.
any related entities must be cascade deleted upon deletion of an entity, like Recipe -> Ingredients, Order -> OrderItem, etc.
`DbContext.Remove(entity)` exists and can handle this well, but we prefer soft-delete (archive, hide) instead of complete data loss; some add this sort of flag `public bool IsDeleted { get; set; }` then with LINQ we can query for entites that are !IsDeleted. **Effectively becoming an UPDATE statement**.

*Global query filters* allow us to specify a Where clause at the model level, i.e. attaching a filter to an entity type and having that filter applied whenever a query on that entity type is executed.

Entity Framework Core in Action, 2nd ed., by Jon P. Smith (Manning, 2021)