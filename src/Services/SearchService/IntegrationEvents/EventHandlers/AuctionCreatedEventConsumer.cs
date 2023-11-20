using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http.Features;
using MongoDB.Entities;
using SearchService.Entities;
using IntegrationEvents.Events;

namespace SearchService.IntegrationEvents.EventHandlers;

public class AuctionCreatedEventConsumer : IConsumer<AuctionCreatedEvent>
{
    private readonly IMapper _mapper;
    
    public AuctionCreatedEventConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuctionCreatedEvent> context)
    {
        var entity = _mapper.Map<AuctionCreatedEvent, AuctionItem>(context.Message);

        await entity.SaveAsync();
    }
}