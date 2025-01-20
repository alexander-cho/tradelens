using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Core.Interfaces;
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
        // var posts = repository.ListAllAsync();
        // return Ok(await posts);

        var posts = context.Stocks
            .Where(x => x.IpoYear == 2007)
            .Where(x => x.Sector == "Finance")
            .Where(x => x.Industry == "Savings Institutions")
            .Where(x => x.Country == "United States")
            .ToListAsync();
        return await posts;
    }

    // [HttpGet("{id:int}")]
    // public async Task<ActionResult<Stock?>> GetCompany(int id)
    // {
    //     var stock = await repository.GetByIdAsync(id);
    //     return stock;
    // }
    
    // possible admin role: add, update, delete companies based on delistings, new ipos, changes, etc.
    
}
