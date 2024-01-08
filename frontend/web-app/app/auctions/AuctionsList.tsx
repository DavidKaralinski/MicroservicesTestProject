'use client'

import { AuctionCard } from "./AuctionCard";
import { ApplicationUrls } from "../common/appConfiguration";
import { Auction, PaginatedResponse } from "../types";
import AppPagination from "../components/AppPagination";
import { get } from "../services/httpRequestService";
import { useEffect, useState } from "react";

export const AuctionsList = () => {
    const [currentPage, setCurrentPage] = useState(1);
    const [totalPages, setTotalPages] = useState<number>(1);
    const [auctions, setAuctions] = useState<Auction[]>([]);

    useEffect(() => {
        get<PaginatedResponse<Auction>>(`${ApplicationUrls.gatewayUrl}/search/AuctionItems?pageSize=4`).then(data => {
            setAuctions(data.results ?? []);
            setTotalPages(data.pageCount);
        });
    }, []);

    useEffect(() => {
        get<PaginatedResponse<Auction>>(`${ApplicationUrls.gatewayUrl}/search/AuctionItems?pageSize=4&pageNumber=${currentPage}`).then(data => {
            setAuctions(data.results ?? []);
            setTotalPages(data.pageCount);
        });
    }, [currentPage]);

    return (
        <>
            <div className='grid grid-cols-4 gap-6'>
                {auctions?.map(x => <AuctionCard key={x.id} auction={x} />) ?? ''}
            </div>
            <div className='flex justify-center mt-5'>
                <AppPagination totalPages={totalPages} currentPage={currentPage} onPageChange={(pageNumber)=>setCurrentPage(pageNumber)} />
            </div>
        </>
    )
}