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
