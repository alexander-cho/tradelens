using Microsoft.AspNetCore.Mvc;
// using Tradelens.Shared.Application;
using Tradelens.Shared.Presentation;
using Tradelens.Social.Application.Posts;

namespace Tradelens.Social.Presentation.Posts;

// look into whether we can make this class internal
[Route("api/v1/[controller]")]
[ApiController]
public sealed class PostsController : ControllerBase
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

    [ActionArguments]
    [HandleException]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<PostResponse>> GetPostById(Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPostByIdQuery(id);
        return Ok(await _getPostByIdQueryHandler.Handle(query, cancellationToken));
    }

    [ActionArguments]
    [HandleException]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PostResponse>>> GetPosts([FromQuery] GetPostsRequest request,
        CancellationToken cancellationToken = default)
    {
        var query = new GetPostsQuery(request.PageSize);
        return Ok(await _getPostsQueryHandler.Handle(query, cancellationToken));
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePost(CreatePostRequest request,
        CancellationToken cancellationToken = default)
    {
        var command = new CreatePostCommand(request.ParentPostId, request.Cashtags, request.Content, request.Sentiment);
        return Ok(await _createPostCommandHandler.Handle(command, cancellationToken));
    }
}