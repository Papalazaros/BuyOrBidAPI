using AutoMapper;
using BuyOrBid.DTO;
using BuyOrBid.Models;
using BuyOrBid.Models.Database;
using BuyOrBid.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IAutoPostService _autoService;
        private readonly IMapper _mapper;

        public SearchController(IElasticClient elasticClient,
            IPostService postService,
            IMapper mapper,
            IAutoPostService autoService)
        {
            _elasticClient = elasticClient;
            _postService = postService;
            _mapper = mapper;
            _autoService = autoService;
        }

        [HttpGet]
        [Route("Autos")]
        public async Task<IActionResult> Search(string? query, [FromQuery] AutoFilterRequest filter, [Range(1, int.MaxValue)] int page = 1, [Range(1, 24)] int pageSize = 5)
        {
            IEnumerable<int>? searchResultIds = null;

            if (!string.IsNullOrEmpty(query))
            {
                ISearchResponse<AutoPostSearchDTO> searchResponse = await _elasticClient.SearchAsync<AutoPostSearchDTO>(
                    s => s.Query(q => q.QueryString(d => d.Query(query)))
                        .From((page - 1) * pageSize)
                        .Size(pageSize));

                searchResultIds = searchResponse.Documents.Select(x => x.PostId);
            }

            IEnumerable<int> filterIds = _autoService.Filter(filter).Select(x => x.PostId);

            if (searchResultIds != null)
            {
                filterIds = searchResultIds.Join(filterIds, x => x, x => x, (postId, _) => postId);
            }

            int totalResults = filterIds.Count();

            filterIds = filterIds.Skip((page - 1) * pageSize).Take(pageSize);

            return Ok(new PaginatedResponse<AutoPost>(await _postService.Get<AutoPost>(filterIds), page, totalResults));
        }

        [HttpGet]
        [Route("Reindex")]
        public async Task<IActionResult> ReIndex()
        {
            await _elasticClient.DeleteByQueryAsync<AutoPostSearchDTO>(q => q.MatchAll());
            IEnumerable<AutoPost> allPosts = await _postService.Get<AutoPost>();
            IEnumerable<AutoPostSearchDTO> postsToIndex = _mapper.Map<IEnumerable<AutoPost>, IEnumerable<AutoPostSearchDTO>>(allPosts);
            await _elasticClient.IndexManyAsync(postsToIndex);

            return Ok($"{postsToIndex.Count()} post(s) reindexed");
        }
    }
}
