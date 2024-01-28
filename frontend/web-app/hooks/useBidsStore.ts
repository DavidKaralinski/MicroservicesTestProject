import { Bid, BidStatus } from "@/types";
import { create } from "zustand";

type State = {
  bids: Bid[];
  isOpen: boolean;
};

type Actions = {
  setBids: (bids?: Bid[]) => void;
  addBid: (bid: Bid) => void;
  setIsOpen: (value: boolean) => void;
  updateBidStatus: (bidId: number, status: BidStatus) => void;
};

export const useBidsStore = create<State & Actions>((set) => ({
  bids: [],
  isOpen: true,

  setBids: (bids?: Bid[]) => {
    set(() => ({
      bids,
    }));
  },

  addBid: (bid: Bid) => {
    set((state) => ({
      bids: !state.bids.find((x) => x.id === bid.id)
        ? [bid, ...state.bids]
        : [...state.bids],
    }));
  },

  setIsOpen: (value: boolean) => {
    set(() => ({
      isOpen: value,
    }));
  },

  updateBidStatus: (bidId: number, status: BidStatus) => {
    set((state) => ({
      bids: state.bids.map((x) => {
        return { ...x, status: x.id === bidId ? status : x.status };
      }),
    }));
  },
}));
