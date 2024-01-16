'use client'

import { useGetAuctionById } from '@/actions/auctionActions'
import Heading from '@/app/components/Heading';
import React from 'react'
import { CountdownTimer } from '../../CountdownTimer';
import CarImage from '../../CarImage';
import AuctionDetails from './AuctionDetails';

export default function AuctionDetailsPage({params}: {params: {id: string}}) {
  const { response: auction, isLoading } = useGetAuctionById(params.id);

  if(isLoading || !auction){
    return <>Loading</>;
  }

  return (
    <div>
      <div className="flex justify-between">
        <Heading title={auction?.make + " " + auction.model} />
        <div className="flex gap-3">
          <h3 className="text-2xl font-semibold">Time remaining:</h3>
          <CountdownTimer endDate={auction.auctionEnd} />
        </div>
      </div>

      <div className="grid grid-cols-2 gap-6 mt-3">
        <div className="w-full bg-gray-200 aspect-h-10 aspect-w-16 rounded-lg overflow-hidden">
          <CarImage imageUrl={auction.imageUrl} />
        </div>

        <div className="border-2 rounded-lg p-2 bg-gray-100">
          <Heading title="Bids" />
        </div>
      </div>

      <div className='mt-3 grid grid-cols-1 rounded-lg'>
        <AuctionDetails auction={auction} />
      </div>
    </div>
  );
}
