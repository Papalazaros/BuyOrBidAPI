using BuyOrBid.Models.Database;
using BuyOrBid.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyOrBid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
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

        [HttpGet]
        [Route("{postId:int}")]
        public async Task<IActionResult> Get(int postId)
        {
            Post post = await _postService.Get<Post>(postId);
            if (post == null) return NotFound();
            return Ok(post);
        }
    }
}
