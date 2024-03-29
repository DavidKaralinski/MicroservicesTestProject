"use client";

import { Auction, PaginatedResponse } from "@/types";
import { httpGet, httpPut, httpPost, invalidatePath, httpDelete } from "./httpActions";
import { useEffect, useMemo, useState } from "react";
import { useSearchParamsStore } from "@/hooks/useSearchParamsStore";
import qs from "query-string";
import toast from "react-hot-toast";

export const useGetAuctions = () => {
  const [response, setResponse] = useState<
    PaginatedResponse<Auction> | undefined
  >(undefined);
  const [isLoading, setIsLoading] = useState(false);

  const params = useSearchParamsStore((state) => ({
    pageNumber: state.pageNumber,
    pageSize: state.pageSize,
    searchTerm: state.searchTerm,
    orderBy: state.orderBy,
    filterBy: state.filterBy,
    winner: state.winner,
    seller: state.seller,
  }));

  const url = useMemo(() => {
    return qs.stringifyUrl({
      url: '/search/AuctionItems',
      query: params,
    });
  }, [params]);

  useEffect(() => {
    if (!url) {
      return;
    }

    setIsLoading(true);

    httpGet<PaginatedResponse<Auction>>(url).then((result) => {
      setResponse(result);
      setIsLoading(false);
    }).catch((error) => {
        setIsLoading(false);
        toast.error(error.message);
      });
  }, [url]);

  return { response, isLoading };
};

export const useGetAuctionById = (id: string) => {
    const [isLoading, setIsLoading] = useState(false);
    const [response, setResponse] = useState<Auction | undefined>(undefined);
  
    useEffect(() => {
        httpGet<Auction>(`/auctions/${id}`).then((result) => {
            setResponse(result);
            setIsLoading(false);
          }).catch((error) => {
              setIsLoading(false);
              toast.error(error.message);
            });
    }, []);
    
  
    return { response, isLoading };
  };

export const useUpdateAuction = () => {
  const [isUpdating, setIsUpdating] = useState(false);
  const [response, setResponse] = useState(false);

  const update = (data: Partial<Auction>) => {
    setIsUpdating(true);
    const url = `/auctions/${data.id}`;

    httpPut(url, data, true)
      .then((r) => {
        invalidatePath('/');
        invalidatePath(`/auctions/details/${data.id}`);
        setIsUpdating(false);
        setResponse(true);
        toast.success('Auction updated');
      })
      .catch((error) => {
        setIsUpdating(false);
        toast.error(error.message);
      });
  };

  return { update, isUpdating, response };
};

export const useDeleteAuction = () => {
  const [isDeleting, setIsDeleting] = useState(false);
  const [response, setResponse] = useState(false);

  const deleteAuction = (id: string) => {
    setIsDeleting(true);
    const url = `/auctions/${id}`;

    httpDelete(url, true)
      .then((r) => {
        invalidatePath('/');
        invalidatePath(`/auctions/details/${id}`);
        setIsDeleting(false);
        setResponse(true);
        toast.success('Auction deleted');
      })
      .catch((error) => {
        setIsDeleting(false);
        toast.error(error.message);
      });
  };

  return { deleteAuction, isDeleting, response };
};

export const useCreateAuction = () => {
  const [response, setResponse] = useState<Auction | undefined>(undefined);
  const [isCreating, setIsCreating] = useState(false);

  const create = (data: Auction) => {
    setIsCreating(true);
    const url = `/auctions`;

    httpPost<Auction>(url, data, true).then((r) => {
      invalidatePath('/');
      setIsCreating(false);
      setResponse(r);
      toast.success('Auction created');
    }).catch((error) => {
        setIsCreating(false);
        toast.error(error.message);
      });;
  };

  return { create, isCreating, response };
};
