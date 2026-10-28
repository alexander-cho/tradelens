using Microsoft.AspNetCore.Mvc;
using Tradelens.Social.Application.Posts;

namespace Tradelens.Social.Presentation.Posts;

[Route("api/v1/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    // inject handler object for each command/query or implement and use mediator

    private readonly CreatePostCommandHandler _createPostCommandHandler;

    public PostsController(CreatePostCommandHandler createPostCommandHandler)
    {
        _createPostCommandHandler = createPostCommandHandler;
    }

    [HttpPost]
    public async Task<ActionResult> CreatePost(CreatePostCommand command, CancellationToken cancellationToken)
    {
        return Ok(await _createPostCommandHandler.Handle(command, cancellationToken));
    }
}