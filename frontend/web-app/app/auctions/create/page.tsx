'use client'

import Heading from '@/app/components/Heading'
import React, { useEffect } from 'react'
import AuctionForm from '../AuctionForm'
import { useCreateAuction } from '@/actions/auctionActions'
import { Auction } from '@/types'
import { useRouter } from 'next/navigation'

export default function CreateAuctionPage() {
  const { create, isCreating, response } = useCreateAuction();
  const router = useRouter();

  const onSubmit = (data: Auction) => {
    create(data);
  }

  useEffect(() => {
    if(response){
      router.push(`/auctions/details/${response.id}`)
    }
  }, [response, router])

  return (
    <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
      <Heading title='Sell a car' subtitle='Fill the details of your car below' />
      <AuctionForm onSubmit={onSubmit} isProcessing={isCreating}/>
    </div>
  )
}
