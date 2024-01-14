import React from 'react'

export default function AuctionDetailsPage({params}: {params: {id: string}}) {
  return (
    <div>Details page for {params.id}</div>
  )
}
