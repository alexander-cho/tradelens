using API.DTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Route("api/company-metrics")]
[ApiController]
public class CompanyMetricsController : ControllerBase
{
    private readonly IGenericRepository<CompanyMetric> _genericRepository;
    private readonly TradelensDbContext _dbContext;

    public CompanyMetricsController(IGenericRepository<CompanyMetric> genericRepository, TradelensDbContext dbContext)
    {
        _genericRepository = genericRepository;
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<CompanyMetricDto>> GetMetrics(
        // [FromQuery] CompanyMetricSpecParams companyMetricSpecParams
        [FromQuery] string ticker,
        [FromQuery] string interval,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] List<string> metric
    )
    {
        var query = _dbContext.CompanyMetrics
            .Where(x => x.Ticker == ticker)
            .Where(x => x.Interval == interval)
            .Where(x => metric.Contains(x.ParentMetric));

        if (from.HasValue)
        {
            query = query.Where(x => x.PeriodEndDate >= from);
        }

        if (to.HasValue)
        {
            query = query.Where(x => x.PeriodEndDate <= to);
        }

        var companyMetrics = await query
            .OrderBy(x => x.ParentMetric)
            .ThenBy(x => x.PeriodEndDate)
            .ToListAsync();

        var companyMetricsAsDto = CompanyMetricMapper.ToCompanyMetricDto(companyMetrics);

        return companyMetricsAsDto;
    }

    [HttpGet("metrics")]
    public async Task<ActionResult<IEnumerable<string>>> GetParentMetricsAssociatedWithTicker([FromQuery] string ticker)
    {
        var metricsList = _dbContext.CompanyMetrics
            .Where(x => x.Ticker == ticker)
            .Select(x => x.ParentMetric)
            .Distinct()
            .ToListAsync();

        return await metricsList;
    }

    [HttpGet("available-companies")]
    public async Task<ActionResult<IEnumerable<string>>> GetCompaniesWithMetricsAvailable()
    {
        var companiesList = _dbContext.CompanyMetrics
            .Select(x => x.Ticker)
            .Distinct()
            .ToListAsync();

        return await companiesList;
    }
    
    // query by Metric (child metric) for functionalities like the chart engine
    [HttpGet("all-metrics")]
    public async Task<ActionResult<CompanyMetricDto>> GetAllMetrics(
        // [FromQuery] CompanyMetricSpecParams companyMetricSpecParams
        [FromQuery] string ticker,
        [FromQuery] string interval,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] string metric
    )
    {
        var query = _dbContext.CompanyMetrics
            .Where(x => x.Ticker == ticker)
            .Where(x => x.Interval == interval)
            .Where(x => metric.Contains(x.ParentMetric + "_" + x.Metric));

        // foreach (var metricName in metric)
        // {
        //     
        // }

        if (from.HasValue)
        {
            query = query.Where(x => x.PeriodEndDate >= from);
        }

        if (to.HasValue)
        {
            query = query.Where(x => x.PeriodEndDate <= to);
        }

        var companyMetrics = await query
            .OrderBy(x => x.ParentMetric)
            .ThenBy(x => x.PeriodEndDate)
            .ToListAsync();

        var companyMetricsAsDto = CompanyMetricMapper.ToCompanyMetricDto(companyMetrics);

        return companyMetricsAsDto;
    }

    [HttpGet("available-metrics")] public async Task<ActionResult<IEnumerable<string>>> GetAllMetricsAssociatedWithTicker([FromQuery] string ticker)
    {
        var metricsList = _dbContext.CompanyMetrics
            .Where(x => x.Ticker == ticker)
            .Select(x => x.ParentMetric + "_" + x.Metric)
            .Distinct()
            .ToListAsync();

        return await metricsList;
    }
}