using API.RequestHelpers;
using Core.Interfaces;
using Core.Models.CompanyFundamentals;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyFundamentalsService _companyFundamentalsService;

    public CompaniesController(ICompanyFundamentalsService companyFundamentalsService)
    {
        _companyFundamentalsService = companyFundamentalsService;
    }
    
    // currently, the business logic for selecting parameters is very redundant, as seen in the service,
    // which tries to do manual API orchestration.
    // Consider creating DB schema with tables (?) for each metric, both common and company specific
    // (Should I do Postgres for common metrics and a NoSql store for company specific ones?)
    // Write a script(s) to periodically populate database with those metrics

    [HttpGet]
    public async Task<ActionResult<CompanyFundamentalsResponse>> GetFundamentalData
    (
        [FromQuery] string ticker,
        [FromQuery] string period,
        // explicitly define to get from query, or else returns 415
        [FromQuery] List<string> metric
    )
    {
        return await _companyFundamentalsService.GetCompanyFundamentalMetricsAsync(ticker, period, metric);
    }

    [Cache(1000)]
    [HttpGet("related-companies")]
    public async Task<ActionResult<HashSet<string>>> GetRelatedCompanies([FromQuery] string ticker)
    {
        return await _companyFundamentalsService.GetRelatedCompaniesAsync(ticker);
    }
    
    [Cache(10000)]
    [HttpGet("company-profile")]
    public async Task<ActionResult<CompanyProfile>> GetCompanyProfile([FromQuery] string ticker)
    {
        return Ok(await _companyFundamentalsService.GetCompanyProfileDataAsync(ticker));
    }
    
    [Cache(10000)]
    [HttpGet("key-metrics")]
    public async Task<ActionResult<KeyMetricsTtm>> GetKeyMetrics([FromQuery] string ticker)
    {
        return Ok(await _companyFundamentalsService.GetKeyMetricsTtmAsync(ticker));
    }
    
    [Cache(10000)]
    [HttpGet("financial-ratios")]
    public async Task<ActionResult<FinancialRatiosTtm>> GetFinancialRatios([FromQuery] string ticker)
    {
        return Ok(await _companyFundamentalsService.GetFinancialRatiosTtmAsync(ticker));
    }
    
    [Cache(10000)]
    [HttpGet("company-profile-finnhub")]
    public async Task<ActionResult<FinnhubCompanyProfile>> GetFinnhubCompanyProfile([FromQuery] string ticker)
    {
        return Ok(await _companyFundamentalsService.GetFinnhubCompanyProfileAsync(ticker));
    }
}