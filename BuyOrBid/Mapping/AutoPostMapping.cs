﻿using AutoMapper;
using BuyOrBid.DTO;
using BuyOrBid.Models.Database;

namespace BuyOrBid.Mapping
{
    public class AutoPostMapping : Profile
    {
        public AutoPostMapping()
        {
            CreateMap<AutoPost, AutoPostSearchDTO>();
        }
    }
}
