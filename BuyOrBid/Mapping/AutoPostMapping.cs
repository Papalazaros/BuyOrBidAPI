using AutoMapper;
using BuyOrBid.Models.Database;

namespace BuyOrBid.Mapping
{
    public class AutoPostMapping : Profile
    {
        public AutoPostMapping()
        {
            CreateMap<AutoPost, AutoPostSearchDto>();
        }
    }
}
