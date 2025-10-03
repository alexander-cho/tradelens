using API.RequestHelpers;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ICompaniesService _companiesService;

    public CompaniesController(ICompaniesService companiesService)
    {
        _companiesService = companiesService;
    }

    [Cache(1000)]
    [HttpGet("related-companies")]
    public async Task<ActionResult<RelatedCompaniesModel>> GetRelatedCompanies(string ticker)
    {
        return await _companiesService.GetRelatedCompaniesAsync(ticker);
    }
}