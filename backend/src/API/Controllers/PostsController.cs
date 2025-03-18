using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class PostsController(IGenericRepository<Post> repository) : BaseApiController
{
    // get all posts
    // TODO: Get all posts for ex, of a specific ticker, that are bullish -> specifications
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetPosts([FromQuery] PostSpecParams postSpecParams)
    {
        // var posts = await _context.Posts
        //     .Where(x => x.Ticker == ticker)
        //     .Where(x => x.Sentiment == sentiment)
        //     .ToListAsync();
        // return posts;

        var spec = new PostSpecification(postSpecParams);
        // var posts = await repository.ListWithSpecAsync(spec);
        // var count = await repository.CountAsync(spec);

        // var pagination = new Pagination<Post>(postSpecParams.PageIndex, postSpecParams.PageSize, count, posts);
        var posts = CreatePagedResult(repository, spec, postSpecParams.PageIndex, postSpecParams.PageSize);
        return await posts;
    }

    // get one post
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Post>> GetPost(int id)
    {
        var post = await repository.GetByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }

        return post;
    }

    // create post
    [HttpPost]
    public async Task<ActionResult<Post>> CreatePost(Post post)
    {
        repository.Add(post);
        if (await repository.SaveAllAsync())
        {
            Console.WriteLine($"Your post about the ticker '{post.Ticker}' has been created.");
            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, post);
        }
        return BadRequest("Could not create post");
    }

    // update post
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Post>> UpdatePost(int id, Post post)
    {
        if (post.Id != id || !PostExists(id))
        {
            return BadRequest("id in route and post Id do not match");
        }

        repository.Update(post);

        if (await repository.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Could not update post");
    }

    // delete post
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<Post>> DeletePost(int id)
    {
        var post = await repository.GetByIdAsync(id);
        if (post == null)
        {
            return NotFound();
        }
        repository.Remove(post);
        if (await repository.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Could not delete product");
    }

    // get list of all tickers mentioned
    [HttpGet("tickers")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTickers()
    {
        var spec = new TickerListSpecification();
        var tickers = await repository.ListWithSpecAsync(spec);
        return Ok(tickers);

        // return Ok(await repository.GetTickers());
    }

    // get list of all possible sentiment
    [HttpGet("sentiments")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetSentiments()
    {
        var spec = new SentimentListSpecification();
        var tickers = await repository.ListWithSpecAsync(spec);
        return Ok(tickers);

        // return Ok(await repository.GetTickers());
    }

    private bool PostExists(int id)
    {
        return repository.Exists(id);
    }
}
