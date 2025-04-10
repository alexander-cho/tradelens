using Core.DTOs.Tradier;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MaxPainController : ControllerBase
{
    private readonly IMaxPainService _service;
    
    public MaxPainController(IMaxPainService service)
    {
        this._service = service;
    }

    [HttpGet]
    public async Task<ActionResult<OptionsData>> GetOptionChains()
    {
        return await _service.GetOptionChainsAsync();
    }
}