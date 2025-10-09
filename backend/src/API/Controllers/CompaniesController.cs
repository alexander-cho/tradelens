using API.RequestHelpers;
using Core.Interfaces;
using Core.Models;
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

    [Cache(1000)]
    [HttpGet("related-companies")]
    public async Task<ActionResult<RelatedCompaniesModel>> GetRelatedCompanies([FromQuery] string ticker)
    {
        return await _companyFundamentalsService.GetRelatedCompaniesAsync(ticker);
    }

    [HttpGet("income-statement")]
    public async Task<ActionResult<IncomeStatement>> GetIncomeStatement([FromQuery] string ticker, string period)
    {
        return await _companyFundamentalsService.GetIncomeStatementAsync(ticker, limit: 5, period);
    }

    [HttpGet("balance-sheet")]
    public async Task<ActionResult<BalanceSheet>> GetBalanceSheet([FromQuery] string ticker, string period)
    {
        try 
        {
            var result = await _companyFundamentalsService.GetBalanceSheetAsync(ticker, limit: 5, period);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
        }
    }
    
    [HttpGet("cash-flow")]
    public async Task<ActionResult<CashFlowStatement>> GetCashFlowStatement([FromQuery] string ticker, string period)
    {
        return await _companyFundamentalsService.GetCashFlowStatementAsync(ticker, limit: 5, period);
    }
}