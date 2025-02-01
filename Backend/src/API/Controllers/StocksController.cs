using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class StocksController : BaseApiController
{
    // private readonly IGenericRepository<Stock> repository;
    private readonly TradeLensDbContext context;

    public StocksController(TradeLensDbContext context)
    {
        // this.repository = repository;
        this.context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Stock>>> GetCompanies()
    {
        var posts = context.Stocks.ToListAsync();
        return Ok(await posts);

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
        var stock = await context.Stocks.FirstOrDefaultAsync(x => x.Ticker == ticker);
        return stock;
    }
    
    // possible admin role: add, update, delete companies based on delistings, new ipos, changes, etc.
    
}
