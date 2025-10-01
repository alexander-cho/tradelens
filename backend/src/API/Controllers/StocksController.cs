using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;

namespace API.Controllers;

public class StocksController(IGenericRepository<Stock> repository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Stock>>> GetCompanies([FromQuery] StockSpecParams stockSpecParams)
    {
        var spec = new StockSpecification(stockSpecParams);
        var stocks = CreatePagedResult(repository, spec, stockSpecParams.PageIndex, stockSpecParams.PageSize);
        return await stocks;

        // var posts = context.Stocks
        //     .Where(x => x.IpoYear == 2007)
        //     .Where(x => x.Sector == "Finance")
        //     .Where(x => x.Industry == "Savings Institutions")
        //     .Where(x => x.Country == "United States")
        //     .ToListAsync();
        // return await posts;
    }
    
    [HttpGet("{ticker}")]
    public async Task<ActionResult<Stock?>> GetCompany(string ticker)
    {
        var spec = new StockByTickerSpecification(ticker.ToUpper());
        var stock = await repository.GetEntityWithSpec(spec);
        if (stock == null)
        {
            return NotFound();
        }

        return stock;
    }
    
    
    // possible admin role: add, update, delete companies based on de-listings, new IPOs, changes, etc.

    
    [HttpGet("ipoYears")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetIpoYears()
    {
        var spec = new IpoYearListSpecification();
        var ipoYears = await repository.ListWithSpecAsync(spec);
        return Ok(ipoYears);
    }
    
    [HttpGet("countries")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetCountries()
    {
        var spec = new CountryListSpecification();
        var countries = await repository.ListWithSpecAsync(spec);
        return Ok(countries);
    }
    
    [HttpGet("sectors")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetSectors()
    {
        var spec = new SectorListSpecification();
        var sectors = await repository.ListWithSpecAsync(spec);
        return Ok(sectors);
    }
}
