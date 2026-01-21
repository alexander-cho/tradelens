using Tradelens.Core.Entities;
using Tradelens.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tradelens.Infrastructure.Data;

namespace Tradelens.Infrastructure.Repositories;

/// <summary>
/// Generic repository for common data access operations.
/// </summary>
/// <remarks>
/// The compiler cannot infer type T of entity to be passed in as a parameter, so we use Set(T)
/// </remarks>
/// <param name="context">The database context.</param>
/// <typeparam name="T">Generic type; BaseEntity</typeparam>
public class GenericRepository<T>(TradelensDbContext context) : IGenericRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Find an entity given the primary key values
    /// </summary>
    /// <param name="id">integer</param>
    /// <returns>One entity with that id</returns>
    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }
    
    /// <summary>
    /// Get all instances of an entity
    /// </summary>
    /// <returns>Readonly list</returns>
    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Add entity to the database
    /// </summary>
    /// <param name="entity">entity with generic type T</param>
    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }

    /// <summary>
    /// Update an entity.
    /// </summary>
    /// <param name="entity">entity with generic type T</param>
    public void Update(T entity)
    {
        context.Set<T>().Update(entity);
        
        // // set the entity first
        // context.Set<T>().Attach(entity);
        // context.Entry(entity).State = EntityState.Modified;
    }
    
    /// <summary>
    /// Delete entity from database
    /// </summary>
    /// <param name="entity">entity with generic type T</param>
    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    /// <summary>
    /// Check if an entity with given id exists, we can use x.Id since it is defined in BaseEntity where T derives from
    /// Compare that against the id that comes in through parameter
    /// </summary>
    /// <param name="id">integer</param>
    /// <returns></returns>
    public bool Exists(int id)
    {
        return context.Set<T>().Any(x => x.Id == id);
    }

    /// <summary>
    /// Persist changes after add, update, delete operations.
    /// </summary>
    /// <returns>boolean value, whether changes were saved</returns>
    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<T?> GetEntityWithSpec(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }
    
    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
    }

    public async Task<IReadOnlyList<TResult>> ListWithSpecAsync<TResult>(ISpecification<T, TResult> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
    }
    
    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), specification);
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification)
    {
        return SpecificationEvaluator<T>.GetQuery<T, TResult>(context.Set<T>().AsQueryable(), specification);
    }

    public async Task<int> CountAsync(ISpecification<T> specification)
    {
        var query = context.Set<T>().AsQueryable();
        query = specification.ApplyCriteria(query);
        return await query.CountAsync();
    }
}
