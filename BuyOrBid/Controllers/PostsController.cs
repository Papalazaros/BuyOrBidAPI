using BuyOrBid.Models.Database;
using BuyOrBid.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BuyOrBid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        public virtual async Task<IActionResult> Create<T>(T post) where T : Post
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            DateTime now = DateTime.UtcNow;
            post.ModifiedDate = now;
            post.CreatedDate = now;
            post.CreatedByUserId = 1;

            return Ok(await _postService.Create(post));
        }

        public async Task<IActionResult> Get<T>(int postId) where T : Post
        {
            T post = await _postService.Get<T>(postId);
            if (post == null) return NotFound();
            return Ok(post);
        }
    }
}
