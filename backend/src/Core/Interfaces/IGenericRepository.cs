using System.Linq.Expressions;
using Core.Entities;

namespace Core.Interfaces;

// each repository, generic or specific will have these methods

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<IReadOnlyList<T>> ListAllAsync();
    Task<T?> GetByIdAsync(int id);
    void Add(T entity);
    void Update(T entity);
    void Remove(T entity);
    bool Exists(int id);
    Task<bool> SaveAllAsync();

    Task<T?> GetEntityWithSpec(ISpecification<T> specification);
    Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> specification);

    // method overload
    Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<T, TResult> specification);
    Task<IReadOnlyList<TResult>> ListWithSpecAsync<TResult>(ISpecification<T, TResult> specification);

    // count number of items for pagination
    Task<int> CountAsync(ISpecification<T> specification);
}
