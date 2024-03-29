﻿using AuctionService.Dtos;
using AuctionService.Entities;
using IntegrationEvents.Events;
using AutoMapper;

namespace AuctionService.Mapping;

public class AuctionServiceMappingProfile : Profile
{
    public AuctionServiceMappingProfile()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<AuctionItem, AuctionDto>();
        CreateMap<Auction, AuctionCreatedEvent>().IncludeMembers(x => x.Item);
        CreateMap<AuctionItem, AuctionCreatedEvent>();

        CreateMap<Auction, AuctionUpdatedEvent>()
            .ForMember(dst => dst.Status, opt => opt.MapFrom(src => Enum.GetName(src.Status)))
            .IncludeMembers(x => x.Item);

        CreateMap<AuctionItem, AuctionUpdatedEvent>();
    }
}
