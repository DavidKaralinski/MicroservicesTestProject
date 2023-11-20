using AutoMapper;
using SearchService.Dtos;
using SearchService.Entities;
using IntegrationEvents.Events;

namespace SearchService.Mapping;

public class SearchServiceMappingProfile : Profile
{
    public SearchServiceMappingProfile()
    {
        CreateMap<AuctionItem, AuctionItemDto>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.ID));

        CreateMap<AuctionCreatedEvent, AuctionItem>()
            .ForMember(dst => dst.ID, opt => opt.MapFrom(src => src.Id.ToString()));
    }
}