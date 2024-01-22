namespace BidService.Enums;

public enum BidStatus : short
{
    Accepted, 
    AcceptedBelowReserve,
    TooLow,
    Finished
}