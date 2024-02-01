"use client";

import { AuctionCard } from "./AuctionCard";
import AppPagination from "../components/AppPagination";
import { useGetAuctions } from "../../actions/auctionActions";
import { useEffect, useMemo, useState } from "react";
import AuctionFilters from "./AuctionFilters";
import { useSearchParamsStore } from "../../hooks/useSearchParamsStore";
import EmptyFilterResults from "../components/EmptyFilterResults";
import { useAuctionsStore } from "@/hooks/useAuctionsStore";

export const AuctionsList = () => {

  const params = useSearchParamsStore(state => ({
    pageNumber: state.pageNumber,
    pageSize: state.pageSize,
    searchTerm: state.searchTerm,
    orderBy: state.orderBy,
    filterBy: state.filterBy,
    winner: state.winner,
    seller: state.seller
  }));


  const setAuctions = useAuctionsStore(state => state.setData);
  const auctions = useAuctionsStore(state => state.auctions);
  const setParams = useSearchParamsStore(state => state.setParams);

  const { response, isLoading } = useGetAuctions();

  useEffect(() => {
    setAuctions(response);
    setParams({ pageCount: response?.pageCount });
  }, [response, setAuctions, setParams]);


  // useEffect(() => {
  //   setIsLoading(true);
  //   console.log(`url = ${url}`);
  //   get<PaginatedResponse<Auction>>(url).then((data) => {
  //     setAuctions(data.results ?? []);
  //     setParams({ pageCount: data.pageCount });
  //     setIsLoading(false);
  //   });
  // }, [url]);

  if(isLoading || !auctions){
    return <>Loading</>
  }

  return (
    <>
      <AuctionFilters />
      {auctions.length === 0 && !isLoading ? (
        <EmptyFilterResults showReset />
      ) : (
        <>
          <div className="grid grid-cols-4 gap-6">
            {auctions?.map((x) => <AuctionCard key={x.id} auction={x} />) ?? ""}
          </div>
          <div className="flex justify-center mt-5">
            <AppPagination />
          </div>
        </>
      )}
    </>
  );
};
