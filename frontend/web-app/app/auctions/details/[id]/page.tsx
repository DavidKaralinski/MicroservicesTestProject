'use client'

import { useDeleteAuction, useGetAuctionById } from '@/actions/auctionActions'
import Heading from '@/app/components/Heading';
import React from 'react'
import { CountdownTimer } from '../../CountdownTimer';
import CarImage from '../../CarImage';
import AuctionDetails from './AuctionDetails';
import { useRouter } from 'next/navigation';
import { Button } from 'flowbite-react';

export default function AuctionDetailsPage({params}: {params: {id: string}}) {
  const { response: auction, isLoading } = useGetAuctionById(params.id);
  const { deleteAuction, isDeleting } = useDeleteAuction();
  const router = useRouter();

  if(isLoading || !auction){
    return <>Loading</>;
  }

  return (
    <div>
      <div className="flex justify-between">
        <div className='flex items-center gap-2'>
          <Heading title={auction?.make + " " + auction.model} />
          {<Button onClick={() => router.push(`/auctions/update/${auction.id}`)} outline>Update</Button>}
          {<Button isProcessing={isDeleting} color='failure' onClick={() => {
            deleteAuction(auction.id);
          }} outline>Delete</Button>}
        </div>
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
