'use client'

import React from 'react'
import Heading from './Heading'
import { Button } from 'flowbite-react'
import { useSearchParamsStore } from '../../hooks/useSearchParamsStore'
import { signIn } from 'next-auth/react'

type Props = {
    title?: string;
    subtitle?: string;
    showReset?: boolean;
    showLogin?: boolean;
    callbackUrl?: string;
}

export default function EmptyFilterResults({
    title = 'No matches for selected filters and search panel text',
    subtitle = 'Try changing or resetting the filter and text in the search panel',
    showReset,
    showLogin,
    callbackUrl
}: Props) {
    const reset = useSearchParamsStore(state => state.reset);

    return (
        <div className='h-[40vh] flex flex-col gap-2 justify-center items-center shadow-lg'>
            <Heading title={title} subtitle={subtitle} center />
            <div className='mt-4'>
                {showReset && (
                    <Button outline onClick={reset}>Remove filters and clear search panel</Button>
                )}
                {showLogin && (
                    <Button outline onClick={() => signIn('id-server', {callbackUrl})}>Login</Button>
                )}
            </div>
        </div>
    )
}