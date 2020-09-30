using BuyOrBid.Models.Database;
using BuyOrBid.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BuyOrBid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AutoPostController : PostController
    {
        private readonly IVinDecodeService _vinDecodeService;
        private readonly IAutoPostService _autoPostService;
        private readonly IPostService _postService;

        public AutoPostController(IPostService postService,
            IVinDecodeService vinDecodeService,
            IAutoPostService autoPostService) : base(postService)
        {
            _vinDecodeService = vinDecodeService;
            _autoPostService = autoPostService;
            _postService = postService;
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

            if (model == null || make == null)
            {
                return BadRequest(ModelState.Values);
            }

            post.SystemTitle = AutoPostService.GenerateTitle(post);

            return await base.Create(post);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _postService.Get<AutoPost>());
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
