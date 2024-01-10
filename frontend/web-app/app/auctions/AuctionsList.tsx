"use client";

import { AuctionCard } from "./AuctionCard";
import { ApplicationUrls } from "../common/appConfiguration";
import { Auction, PaginatedResponse } from "../types";
import AppPagination from "../components/AppPagination";
import { get } from "../services/httpRequestService";
import { useEffect, useState } from "react";
import AuctionFilters from "./AuctionFilters";
import { useSearchParamsStore } from "../hooks/useSearchParamsStore";
import EmptyFilterResults from "../components/EmptyFilterResults";
import { Spinner } from "flowbite-react";

export const AuctionsList = () => {
  const [auctions, setAuctions] = useState<Auction[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  const { searchValue, pageNumber, pageSize, setParams, orderBy } =
    useSearchParamsStore();

  useEffect(() => {
    setIsLoading(true);

    get<PaginatedResponse<Auction>>(
      `${
        ApplicationUrls.gatewayUrl
      }/search/AuctionItems?pageSize=${pageSize}&pageNumber=${pageNumber}${
        searchValue ? "&filterByMake=" + searchValue : ""
      }`
    ).then((data) => {
      setAuctions(data.results ?? []);
      setParams({ pageCount: data.pageCount });
      setIsLoading(false);
    });
  }, [pageNumber, pageSize, searchValue]);

  if(!isLoading){
    return <Spinner title="Loading" />
  }

  return (
    <>
      <AuctionFilters />
      {auctions.length === 0 ? (
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
