using Microsoft.AspNetCore.Mvc;
using Tradelens.Shared.Application;
using Tradelens.Social.Application.Posts;

namespace Tradelens.Social.Presentation.Posts;

[Route("api/v1/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PostsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult> CreatePost(CreatePostCommand request, CancellationToken cancellationToken = default)
    {
        return Ok(await _mediator.Send(request, cancellationToken));
    }
}
