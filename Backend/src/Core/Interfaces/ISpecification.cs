using System;
using System.Linq.Expressions;

namespace Core.Interfaces;

// define the methods that the specification pattern will support

public interface ISpecification<T>
{
    // to support the Where() function, Criteria to be passed into an evaluator
    Expression<Func<T, bool>>? Criteria { get; }

    // return type of object, since we can order by different criteria, e.g. alphabetical, numerical, time, etc.
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    
    bool IsDistinct { get; }

    // pagination: for example, to get page 3 while setting items per page to 5, Skip(10) and Take(5)
    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }

    IQueryable<T> ApplyCriteria(IQueryable<T> query);
}


// takes in type T but returns a result that is not
// i.e. getting the list of tickers that exist which is a list of strings, not an entity type nor a list of it
public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T, TResult>>? Select { get; }
}
