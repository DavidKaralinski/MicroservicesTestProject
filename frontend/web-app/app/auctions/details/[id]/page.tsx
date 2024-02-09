'use client'

import { useDeleteAuction, useGetAuctionById } from '@/actions/auctionActions'
import Heading from '@/app/components/Heading';
import React, { useEffect } from 'react'
import { CountdownTimer } from '../../CountdownTimer';
import CarImage from '../../CarImage';
import AuctionDetails from './AuctionDetails';
import { useRouter } from 'next/navigation';
import { Button } from 'flowbite-react';
import BidsList from './BidsList';
import { useGetCurrentUser } from '@/actions/bidActions';

export default function AuctionDetailsPage({params}: {params: {id: string}}) {
  const { response: auction, isLoading } = useGetAuctionById(params.id);
  const { deleteAuction, isDeleting, response } = useDeleteAuction();
  const router = useRouter();
  const { currentUser, isLoading: isCurrentUserLoading } = useGetCurrentUser();

  useEffect(() => {
    if(response == true){
      router.push('/');
    }
  },  [response, router]);

  if(isLoading || !auction || isCurrentUserLoading){
    return <>Loading</>;
  }

  return (
    <div>
      <div className="flex justify-between">
        <div className='flex items-center gap-2'>
          <Heading title={auction?.make + " " + auction.model} />
          {auction.sellerName === currentUser?.name && 
           <Button onClick={() => router.push(`/auctions/update/${auction.id}`)} outline>Update</Button>}
          {auction.sellerName === currentUser?.name && 
          <Button isProcessing={isDeleting} color='failure' onClick={() => {
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

        <BidsList user={currentUser} auction={auction} />
      </div>

      <div className='mt-3 grid grid-cols-1 rounded-lg'>
        <AuctionDetails auction={auction} />
      </div>
    </div>
  );
}
