import EmptyFilterResults from '@/app/components/EmptyFilterResults'
import React from 'react'

export default function SignInPage({searchParams}: {searchParams: {callbackUrl: string}}) {
  return (
    <EmptyFilterResults 
        title='You need to be logged in to see this page'
        subtitle='Please click below to sign in'
        showLogin={true}
        callbackUrl={searchParams.callbackUrl}
    />
  )
}
