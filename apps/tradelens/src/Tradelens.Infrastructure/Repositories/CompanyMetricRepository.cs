using Tradelens.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tradelens.Infrastructure.Data;

namespace Tradelens.Infrastructure.Repositories;

public class CompanyMetricRepository : ICompanyMetricRepository
{
    private readonly TradelensDbContext _context;
    
    public CompanyMetricRepository(TradelensDbContext context)
    {
        _context = context;
    }
    public Task<IEnumerable<string>> GetListOfPossibleMetricsAsync(string ticker)
    {
        var query = _context.CompanyMetrics.AsQueryable();

        query = query.Where(x => x.Ticker == ticker);

        // return await query.ToListAsync();

        throw new NotImplementedException();
    }
}