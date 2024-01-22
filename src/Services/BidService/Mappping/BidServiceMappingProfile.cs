using AutoMapper;
using BidService.Entities;
using BidService.Models;
using IntegrationEvents.Events;

namespace BidService.Mapping;

public class BidServiceMappingProfile : Profile
{
    public BidServiceMappingProfile()
    {
        CreateMap<Bid, BidModel>()
            .ForMember(dst => dst.Status, opt => opt.MapFrom(src => Enum.GetName(src.Status)));

        CreateMap<Bid, BidPlacedEvent>()
            .ForCtorParam(nameof(BidPlacedEvent.AuctionId), opt => opt.MapFrom(src => src.AuctionId))
            .ForCtorParam(nameof(BidPlacedEvent.BidStatus), opt => opt.MapFrom(src => src.Status))
            .ForCtorParam(nameof(BidPlacedEvent.BidderName), opt => opt.MapFrom(src => src.BidderName))
            .ForCtorParam(nameof(BidPlacedEvent.BidTime), opt => opt.MapFrom(src => src.BidTime))
            .ForCtorParam(nameof(BidPlacedEvent.BidAmount), opt => opt.MapFrom(src => src.Amount));
    }
}