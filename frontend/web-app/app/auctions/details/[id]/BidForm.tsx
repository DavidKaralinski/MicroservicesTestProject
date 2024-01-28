'use client'

type Props = {
    auctionId: string;
    highestBid: number;
}

import { useCreateBid } from '@/actions/bidActions';
import { useBidsStore } from '@/hooks/useBidsStore';
import { Bid } from '@/types';
import { formatNumberToCommaSeparated } from '@/utils';
import React, { useEffect } from 'react'
import { FieldValues, useForm } from 'react-hook-form';
import { toast } from 'react-hot-toast';

export default function BidForm({ auctionId, highestBid }: Props) {
    const {register, handleSubmit, reset, formState: {errors}} = useForm();
    const { create, isCreating, response } = useCreateBid();

    const addBid = useBidsStore(state => state.addBid);

    function onSubmit(data: FieldValues) {
        if (data.amount <= highestBid) {
            reset();
            return toast.error('Bid must be at least $' + formatNumberToCommaSeparated(highestBid + 1))
        }

        create({...data, auctionId: auctionId} as Bid);
    }

    useEffect(() => {
        if(response){
            addBid(response);
        }
    }, [response]);

    return (
        <form onSubmit={handleSubmit(onSubmit)} className='flex items-center border-2 rounded-lg py-2'>
            <input 
                type="number" 
                {...register('amount')}
                className='input-custom text-sm text-gray-600'
                placeholder={`Enter your bid (minimum bid is $${formatNumberToCommaSeparated(highestBid + 1)})`}
            />
        </form>
    )
}