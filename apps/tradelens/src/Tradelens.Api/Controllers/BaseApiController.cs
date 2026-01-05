using Microsoft.AspNetCore.Mvc;
using Tradelens.Core.Entities;
using Tradelens.Core.Interfaces;
using Tradelens.Api.RequestHelpers;

namespace Tradelens.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class BaseApiController : ControllerBase
{
    // implement pagination for any other derived controllers
    protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repository, ISpecification<T> specification, int pageIndex, int pageSize) where T : BaseEntity
    {
        var items = await repository.ListWithSpecAsync(specification);
        var count = await repository.CountAsync(specification);
        var pagination = new Pagination<T>(pageIndex, pageSize, count, items);
        return Ok(pagination);
    }
}
