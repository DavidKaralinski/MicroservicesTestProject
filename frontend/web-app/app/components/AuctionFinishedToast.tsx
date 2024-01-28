'use client'

import { useGetAuctionById } from '@/actions/auctionActions'
import { Auction, AuctionFinishedNotification } from '@/types'
import { formatNumberToCommaSeparated } from '@/utils'
import Image from 'next/image'
import Link from 'next/link'
import React from 'react'

type Props = {
    notification: AuctionFinishedNotification
}

export default function AuctionFinishedToast({ notification }: Props) {
    const { response: auction, isLoading } = useGetAuctionById(notification.auctionId);

    if(isLoading || !auction){
        return <>Loading...</>;
    }

    return (
        <Link href={`/auctions/details/${auction.id}`} className='flex flex-col items-center'>
            <div className='flex flex-row items-center gap-2'>
                <Image
                    src={auction.imageUrl}
                    alt='image'
                    height={80}
                    width={80}
                    className='rounded-lg w-auto h-auto'
                />
                <div className='flex flex-col'>
                    <span>Auction for {auction.make} {auction.model} has finished</span>
                    {notification.isSold && notification.amount ? (
                        <p>Congrats to {notification.winnerName} who has won this auction for 
                            $${formatNumberToCommaSeparated(notification.amount)}</p>
                    ) : (
                        <p>This item hasnt been sold</p>
                    )}
                </div>

            </div>
        </Link>
    )
}