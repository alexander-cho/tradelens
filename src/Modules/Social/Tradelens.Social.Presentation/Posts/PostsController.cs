using Microsoft.AspNetCore.Mvc;
// using Tradelens.Shared.Application;
using Tradelens.Shared.Presentation;
using Tradelens.Social.Application.Posts;

namespace Tradelens.Social.Presentation.Posts;

[Route("api/v1/[controller]")]
[ApiController]
public class PostsController : ControllerBase
{
    private readonly CreatePostCommandHandler _createPostCommandHandler;
    private readonly GetPostByIdQueryHandler _getPostByIdQueryHandler;
    private readonly GetPostsQueryHandler _getPostsQueryHandler;

    public PostsController(CreatePostCommandHandler createPostCommandHandler,
        GetPostByIdQueryHandler getPostByIdQueryHandler,
        GetPostsQueryHandler getPostsQueryHandler)
    {
        _createPostCommandHandler = createPostCommandHandler;
        _getPostByIdQueryHandler = getPostByIdQueryHandler;
        _getPostsQueryHandler = getPostsQueryHandler;
    }

    // private readonly IMediator _mediator;

    // public PostsController(IMediator mediator)
    // {
    //     _mediator = mediator;
    // }

    // Think about: If it's ActionResult<PostId>, the presentation layer knows about a domain concept, which is
    // not unreasonable since there is a transitive dependency.
    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePost(CreatePostCommand request,
        CancellationToken cancellationToken = default)
    {
        var response = await _createPostCommandHandler.Handle(request, cancellationToken);
        return Ok(response.Value);
    }

    [ActionArguments]
    [HandleException]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<PostResponse>> GetPostById(Guid id,
        CancellationToken cancellationToken = default)
    {
        var q = new GetPostByIdQuery(id);
        return Ok(await _getPostByIdQueryHandler.Handle(q, cancellationToken));
    }

    [ActionArguments]
    [HandleException]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostResponse>>> GetPosts(GetPostsQuery request,
        CancellationToken cancellationToken = default)
    {
        return Ok(await _getPostsQueryHandler.Handle(request, cancellationToken));
    }
}