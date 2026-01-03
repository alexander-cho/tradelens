using System.Linq.Expressions;
using Tradelens.Core.Entities;
using Tradelens.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tradelens.Infrastructure.Data;

namespace Tradelens.Infrastructure.Repositories;

// generic repo for crud. compiler cannot infer type <T> of entity to be passed in as a parameter,
// so Set<T> the type

public class GenericRepository<T>(TradelensDbContext context) : IGenericRepository<T> where T : BaseEntity
{
    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id);
    }

    public void Add(T entity)
    {
        context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        // set the entity first
        context.Set<T>().Attach(entity);
        context.Entry(entity).State = EntityState.Modified;
    }
    
    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public bool Exists(int id)
    {
        return context.Set<T>().Any(x => x.Id == id);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<T?> GetEntityWithSpec(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> specification)
    {
        return SpecificationEvaluator<T>.GetQuery(context.Set<T>().AsQueryable(), specification);
    }

    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TResult>> ListWithSpecAsync<TResult>(ISpecification<T, TResult> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
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
