using AutoMapper;
using BuyOrBid.Models.Database;
using BuyOrBid.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyOrBid.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IElasticClient _elasticClient;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public SearchController(IElasticClient elasticClient,
            IPostService postService,
            IMapper mapper)
        {
            _elasticClient = elasticClient;
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("Autos")]
        public async Task<IActionResult> Search(string? query, int page = 0, int pageSize = 5)
        {
            ISearchResponse<AutoPostSearchDto> searchResponse = await _elasticClient.SearchAsync<AutoPostSearchDto>(
                s => s.Query(q => q.QueryString(d => d.Query(query)))
                    .From(page * pageSize)
                    .Size(pageSize));

            var postIds = searchResponse.Documents.Select(x => x.PostId);

            return Ok(_postService.GetAll<AutoPost>(postIds));
        }

        [HttpGet]
        [Route("Reindex")]
        public async Task<IActionResult> ReIndex()
        {
            await _elasticClient.DeleteByQueryAsync<AutoPostSearchDto>(q => q.MatchAll());
            var allPosts = await _postService.GetAll<AutoPost>();
            IEnumerable<AutoPostSearchDto> postsToIndex = _mapper.Map<IEnumerable<AutoPost>, IEnumerable<AutoPostSearchDto>>(allPosts);
            await _elasticClient.IndexManyAsync(postsToIndex);

            return Ok($"{postsToIndex.Count()} post(s) reindexed");
        }
    }
}
