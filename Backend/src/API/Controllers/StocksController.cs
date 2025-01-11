using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Infrastructure.Data;

namespace API.Controllers;

public class StocksController : BaseApiController
{
    private readonly TradeLensDbContext _context;
    public StocksController(TradeLensDbContext context)
    {
        _context = context;
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Stock?>> GetTicker(int id)
    {
        var stock = await _context.Stocks.FindAsync(id);
        return stock;
    }
}