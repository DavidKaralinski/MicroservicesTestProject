import { Auction, PaginatedResponse } from "@/types"
import { create } from "zustand"

type State = {
    auctions: Auction[]
    totalCount: number
    pageCount: number
}

type Actions = {
    setData: (data?: PaginatedResponse<Auction>) => void;
    setCurrentPrice: (auctionId: string, amount: number) => void;
    removeAuction: (auctionId: string) => void;
}

const initialState: State = {
    auctions: [],
    pageCount: 0,
    totalCount: 0
}

export const useAuctionsStore = create<State & Actions>((set) => ({
    ...initialState,

    setData: (data?: PaginatedResponse<Auction>) => {
        set(() => ({
            auctions: data?.results,
            totalCount: data?.totalCount,
            pageCount: data?.pageCount
        }))
    },

    setCurrentPrice: (auctionId: string, amount: number) => {
        set((state) => ({
            auctions: state.auctions.map((auction) => auction.id === auctionId 
                ? {...auction, currentHighBid: amount} : auction)
        }))
    },

    removeAuction: (auctionId: string) => {
        set((state) => ({
            auctions: state.auctions.filter(x => x.id !== auctionId),
            totalCount: state.totalCount,
        }))
    }
}))