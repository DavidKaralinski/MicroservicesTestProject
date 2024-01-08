import { CountdownTimer } from "./CountdownTimer";
import CarImage from "./CarImage";
import { Auction } from "../types";

type AuctionCardProps = {
    auction: Auction;
};

export const AuctionCard = (props: AuctionCardProps) => {

    return (
        <a href='#' className='group'>
            <div className='w-full bg-gray-200 aspect-w-16 aspect-h-10 rounded-lg overflow-hidden'>
                <div>
                    <CarImage imageUrl={props.auction.imageUrl}/>
                    <div className='absolute bottom-2 left-2'>
                        <CountdownTimer endDate={"2024-11-30T15:30:23.613Z"}/>
                    </div>
                </div>
            </div>
            <div className='flex justify-between items-center mt-4'>
                    <h3 className='text-gray-700'>{props.auction.make} {props.auction.model}</h3>
                    <p className='font-semibold text-sm'>{props.auction.year}</p>
            </div>
        </a>
    )
}