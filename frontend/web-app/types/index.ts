export type PaginatedResponse<T> = {
  results?: T[];
  totalCount: number;
  pageCount: number;
};

export type Auction = {
  id: string;
  reservePrice: number;
  sellerName: string;
  winner?: string;
  soldAmount?: number;
  currentHighBid?: number;
  createdAt: string;
  updatedAt?: string;
  auctionEnd: string;
  status: string;
  make: string;
  model: string;
  year: number;
  color: string;
  mileage: number;
  imageUrl: string;
};

export enum BidStatus {
  Accepted = "Accepted",
  AcceptedBelowReserve = "AcceptedBelowReserve",
  TooLow = "TooLow",
  Finished = "Finished"
}

export type Bid = {
  id: number;
  auctionId: string;
  amount: number;
  bidTime: string;
  bidderName: string;
  status: BidStatus
}

export type BidPlacedNotification = {
  bidId: number;
  auctionId: string;
  bidAmount: number;
  bidTime: string;
  bidderName: string;
  bidStatus: BidStatus
}

export type AuctionFinishedNotification = {
  auctionId: string;
  isSold: boolean;
  amount?: number;
  winnerName?: string;
  sellerName?: string;
}

export type AuctionDeletedNotification = {
  id: string;
}

export type AcceptedBidStatusChangedNotification = {
  bidId: number;
  auctionId: string;
  bidderName: string;
  bidAmount: number;
  bidStatus: BidStatus;
  bidTime: string;
}
