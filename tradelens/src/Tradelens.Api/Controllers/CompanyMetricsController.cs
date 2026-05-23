// using Tradelens.Core.Entities;
// using Tradelens.Core.Interfaces;
// using Tradelens.Core.Specifications;
using Tradelens.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tradelens.Api.DTOs;

namespace Tradelens.Api.Controllers;

[Route("api/company-metrics")]
[ApiController]
public class CompanyMetricsController : ControllerBase
{
    // private readonly IGenericRepository<CompanyMetric> _genericRepository;
    private readonly TradelensDbContext _dbContext;

    public CompanyMetricsController(
        // IGenericRepository<CompanyMetric> genericRepository,
        TradelensDbContext dbContext)
    {
        // _genericRepository = genericRepository;
        _dbContext = dbContext;
    }

    [HttpGet("grouped-parent")]
    public async Task<ActionResult<CompanyMetricWithParentDto>> GetMetricsGroupedByParent(
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

        var companyMetricsAsDto = CompanyMetricWithParentMapper.ToCompanyMetricDto(companyMetrics);

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
            .Where(x => x.Interval == interval);

        // foreach (var metricName in metric)
        // {
        //     
        // }
        
        if (metric.Contains('_'))
        {
            var splitMetricToSend = metric.Split('_');
            query = query.Where(x => x.ParentMetric == splitMetricToSend[0] && x.Metric == splitMetricToSend[1]);
        }
        else
        {
            query = query.Where(x => x.ParentMetric == metric && x.Metric == metric);
        }

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

    [HttpGet("available-metrics")]
    public async Task<ActionResult<IEnumerable<string>>> GetAllMetricsAssociatedWithTicker([FromQuery] string ticker)
    {
        var metricsList = _dbContext.CompanyMetrics
            .Where(x => x.Ticker == ticker)
            .Select(x => x.ParentMetric + "_" + x.Metric)
            .Distinct()
            .ToListAsync();

        var metrics = await metricsList;

        var cleanedMetrics = metrics.Select(x =>
        {
            var splitMetric = x.Split('_');
            if (splitMetric[0] == splitMetric[1])
            {
                return splitMetric[0];
            }
            return x;
        }).ToList();

        return cleanedMetrics;
    }
}