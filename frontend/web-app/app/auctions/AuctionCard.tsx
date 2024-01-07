import { Auction } from "./Auction";
import Image from 'next/image'
import { CountdownTimer } from "./CountdownTimer";

type AuctionCardProps = {
    auction: Auction;
};

export const AuctionCard = (props: AuctionCardProps) => {

    return (
        <a href='#'>
            <div className='w-full bg-gray-200 aspect-w-16 aspect-h-10 rounded-lg overflow-hidden'>
                <div>
                    <Image 
                        src={props.auction.imageUrl}
                        alt='image'
                        fill
                        priority
                        className='object-cover'
                        sizes='(max-width:768px) 100vw, (max-width:1200px) 50vw, 25vw'
                    />
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