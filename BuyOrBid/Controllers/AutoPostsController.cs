﻿using BuyOrBid.Models.Database;
using BuyOrBid.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BuyOrBid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutoPostsController : PostsController
    {
        private readonly IVinDecodeService _vinDecodeService;
        private readonly IAutoPostService _autoPostService;
        private readonly IPostService _postService;

        public AutoPostsController(IPostService postService,
            IVinDecodeService vinDecodeService,
            IAutoPostService autoPostService) : base(postService)
        {
            _vinDecodeService = vinDecodeService;
            _autoPostService = autoPostService;
            _postService = postService;
        }

        [HttpGet]
        [Route("{postId:int}")]
        public async Task<IActionResult> Get(int postId)
        {
            return await base.Get<AutoPost>(postId);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AutoPost post)
        {
            Model? model = null;
            Make? make = null;

            if (post.ModelId.HasValue)
            {
                model = await _autoPostService.GetModel(post.ModelId.Value);
            }

            if (post.MakeId.HasValue)
            {
                make = await _autoPostService.GetMake(post.MakeId.Value);
            }

            post.SystemTitle = AutoPostService.GenerateTitle(post);

            return await base.Create(post);
        }

        [HttpGet]
        [Route("Vin/Validate")]
        public bool Validate(string vin)
        {
            return _vinDecodeService.IsValid(vin);
        }

        [HttpGet]
        [Route("Vin/Decode")]
        public async Task<IActionResult> Decode(string vin, int? modelYear = null)
        {
            return Ok(await _autoPostService.CreatePostsFromVin(vin, modelYear));
        }
    }
}
