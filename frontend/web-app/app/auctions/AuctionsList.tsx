import { get } from "../services/HttpRequestService";
import { AuctionCard } from "./AuctionCard";
import { ApplicationUrls } from "../common/AppConfiguration";
import { Auction } from "./Auction";
import { PaginatedResponse } from "../common/PaginatedResponse";

export const AuctionsList = async () => {
    const response = await get<PaginatedResponse<Auction>>(`${ApplicationUrls.gatewayUrl}/search/AuctionItems`);

    return (
        <div className='grid grid-cols-4 gap-6'>
            {response.results?.map(x => <AuctionCard key={x.id} auction={x} />) ?? ''}
        </div>
    )
}