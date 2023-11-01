using AuctionService.Dtos;
using AuctionService.Entities;
using AutoMapper;

namespace AuctionService.Mapping;

public class AuctionServiceMappingProfile : Profile
{
    public AuctionServiceMappingProfile()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<AuctionItem, AuctionDto>();
    }
}
