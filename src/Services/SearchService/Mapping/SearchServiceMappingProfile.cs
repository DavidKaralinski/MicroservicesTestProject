using AutoMapper;
using SearchService.Dtos;
using SearchService.Entities;

namespace SearchService.Mapping;

public class SearchServiceMappingProfile : Profile
{
    public SearchServiceMappingProfile()
    {
        CreateMap<AuctionItem, AuctionItemDto>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.ID));
    }
}