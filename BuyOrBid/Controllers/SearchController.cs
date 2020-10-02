using AutoMapper;
using BuyOrBid.DTO;
using BuyOrBid.Models;
using BuyOrBid.Models.Database;
using BuyOrBid.Services;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public async Task<IActionResult> Search(string? query, [Range(1, int.MaxValue)]int page = 1, [Range(1, 24)]int pageSize = 5)
        {
            ISearchResponse<AutoPostSearchDto> searchResponse = await _elasticClient.SearchAsync<AutoPostSearchDto>(
                s => s.Query(q => q.QueryString(d => d.Query(query)))
                    .From((page - 1) * pageSize)
                    .Size(pageSize));

            IEnumerable<int> postIds = searchResponse.Documents.Select(x => x.PostId);

            return Ok(new PaginatedResponse<AutoPost>(await _postService.Get<AutoPost>(postIds), page, searchResponse.Total));
        }

        //[HttpGet]
        //[Route("Reindex")]
        //public async Task<IActionResult> ReIndex()
        //{
        //    await _elasticClient.DeleteByQueryAsync<AutoPostSearchDto>(q => q.MatchAll());
        //    IEnumerable<AutoPost> allPosts = await _postService.Get<AutoPost>();
        //    IEnumerable<AutoPostSearchDto> postsToIndex = _mapper.Map<IEnumerable<AutoPost>, IEnumerable<AutoPostSearchDto>>(allPosts);
        //    await _elasticClient.IndexManyAsync(postsToIndex);

        //    return Ok($"{postsToIndex.Count()} post(s) reindexed");
        //}
    }
}
