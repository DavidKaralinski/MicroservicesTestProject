'use client'

import Heading from '@/app/components/Heading'
import { Auction } from '@/types'
import { User } from 'next-auth'
import React, { useEffect } from 'react'
import BidForm from './BidForm'
import { useGetBidsByAuctionId } from '@/actions/bidActions'
import { formatNumberToCommaSeparated } from '@/utils'
import EmptyFilterResults from '@/app/components/EmptyFilterResults'
import { BidItem } from './BidItem'
import { useBidsStore } from '@/hooks/useBidsStore'

type Props = {
    user: User | undefined
    auction: Auction
}

export default function BidsList({ user, auction }: Props) {
    const { response: bidsData, isLoading } = useGetBidsByAuctionId(auction.id);

    const bids = useBidsStore(state => state.bids);
    const isOpen = useBidsStore(state => state.isOpen);
    const setIsOpen = useBidsStore(state => state.setIsOpen);
    const openForBids = new Date(auction.auctionEnd) > new Date();
    const setBids = useBidsStore(state => state.setBids);

    useEffect(() => {
        setIsOpen(openForBids);
    }, [openForBids, setIsOpen]);

    useEffect(() => {
        setBids(bidsData);
    }, [bidsData, setBids]);

    const highestBid = bids?.reduce((prev, current) => prev > current.amount
        ? prev
        : current.status?.toString().includes('Accepted')
            ? current.amount
            : prev, 0);

    if (isLoading) return <span>Loading bids...</span>

    return (
        <div className='rounded-lg shadow-md'>
            <div className='py-2 px-4 bg-white'>
                <div className='sticky top-0 bg-white p-2'>
                    <Heading title={`Current high bid is $${formatNumberToCommaSeparated(highestBid ?? 0)}`} />
                </div>
            </div>

            <div className='overflow-auto h-[400px] flex flex-col-reverse px-2'>
                {bids?.length === 0 ? (
                    <EmptyFilterResults title='No bids for this item'
                        subtitle='Please feel free to make a bid' />
                ) : (
                    <>
                        {bids?.map(bid => (
                            <BidItem key={bid.id} bid={bid} />
                        ))}
                    </>
                )}
            </div>

            <div className='px-2 pb-2 text-gray-500'>
                {!isOpen ? (
                    <div className='flex items-center justify-center p-2 text-lg font-semibold'>
                        This auction has finished
                    </div>
                ) : !user ? (
                    <div className='flex items-center justify-center p-2 text-lg font-semibold'>
                        Please login to make a bid
                    </div>
                ) : user && user.username === auction.sellerName ? (
                    <div className='flex items-center justify-center p-2 text-lg font-semibold'>
                        You cannot bid on your own auction
                    </div>
                ) : (
                    <BidForm auctionId={auction.id} highestBid={highestBid ?? 0} />
                )}
            </div>
        </div>
    )
}