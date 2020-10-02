using AutoMapper;
using BuyOrBid.DTO;
using BuyOrBid.Models;
using BuyOrBid.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyOrBid.Services
{
    public interface IPostService
    {
        Task<T> Get<T>(int postId) where T : Post;
        Task<IEnumerable<T>> Get<T>(IEnumerable<int>? ids = null) where T : Post;
        Task<T> Create<T>(T post) where T : Post;
    }

    public class PostService : IPostService
    {
        private readonly MyDbContext _myDbContext;
        private readonly IElasticClient _elasticClient;
        private readonly IMapper _mapper;

        public PostService(MyDbContext myDbContext,
            IMapper mapper,
            IElasticClient elasticClient)
        {
            _myDbContext = myDbContext;
            _elasticClient = elasticClient;
            _mapper = mapper;
        }

        public async Task<T> Create<T>(T post) where T : Post
        {
            await _myDbContext.AddAsync(post);
            await _myDbContext.SaveChangesAsync();

            if (post.IsPublic == true && post is AutoPost autoPost)
            {
                AutoPostSearchDto postToIndex = _mapper.Map<T, AutoPostSearchDto>(post);
                await _elasticClient.IndexDocumentAsync(postToIndex);
            }

            return post;
        }

        public async Task<T> Get<T>(int postId) where T : Post
        {
            return await _myDbContext.Set<T>().AsNoTracking().Include(x => x.PostImages).FirstOrDefaultAsync(x => x.PostId == postId);
        }

        public async Task<IEnumerable<T>> GetAll<T>() where T : Post
        {
            return await _myDbContext.Set<T>().ToArrayAsync();
        }

        public async Task<IEnumerable<T>> Get<T>(IEnumerable<int>? ids = null) where T : Post
        {
            if (ids is null) return await _myDbContext.Set<T>().AsNoTracking().ToArrayAsync();
            return await _myDbContext.Set<T>().AsNoTracking().Where(x => ids.Contains(x.PostId)).Include(x => x.PostImages).ToArrayAsync();
        }
    }
}
