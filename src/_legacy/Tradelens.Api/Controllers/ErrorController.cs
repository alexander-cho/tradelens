using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tradelens.Api.DTOs;

namespace Tradelens.Api.Controllers;

public class ErrorController : BaseApiController
{
    // permissions
    [HttpGet("unauthorized")]
    public IActionResult GetUnauthorized()
    {
        return Unauthorized();
    }

    [HttpGet("bad-request")]
    public IActionResult GetBadRequest()
    {
        return BadRequest("bad request");
    }

    [HttpGet("not-found")]
    public IActionResult GetNotFound()
    {
        return NotFound();
    }

    [HttpGet("internal-error")]
    public IActionResult GetInternalError()
    {
        throw new Exception("test exception");
    }

    [HttpPost("validation-error")]
    public IActionResult GetValidationError(CreatePostDto post)
    {
        return Ok();
    }

    [Authorize]
    [HttpGet("secret")]
    public IActionResult GetSecret()
    {
        var name = User.FindFirst(ClaimTypes.Name)?.Value;
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Ok("Hello " + name + ", your id is: " + id);
    }
}
