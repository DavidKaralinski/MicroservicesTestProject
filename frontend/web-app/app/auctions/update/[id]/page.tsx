'use client'

import { useGetAuctionById, useUpdateAuction } from '@/actions/auctionActions'
import Heading from '@/app/components/Heading';
import React, { useEffect } from 'react'
import AuctionForm from '../../AuctionForm';
import { Auction } from '@/types';
import { useRouter } from 'next/navigation';

export default function UpdateAuctionPage({params}: {params: {id: string}}) {
  const { response: auction, isLoading } = useGetAuctionById(params.id);
  const { update, isUpdating, response } = useUpdateAuction();
  const router = useRouter();

  const onSubmit = (data: Auction) => {
    update(data);
  }

  useEffect(() => {
    if(response === true){
      router.push(`../details/${params.id}`);
    }
  },  [response, params.id, router]);

  if(isLoading){
    return <>Loading</>;
  }

  return (
    <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
      <Heading title='Update your auction' subtitle='Update the details of your car below' />
      <AuctionForm data={auction} isProcessing={isUpdating} onSubmit={onSubmit} />
    </div>
  );
}
