"use client";

import { Bid } from "@/types";
import { httpGet, httpPost, invalidatePath, httpDelete } from "./httpActions";
import { useEffect, useState } from "react";
import { applicationUrls } from "@/common/appConfiguration";
import toast from "react-hot-toast";
import { getCurrentUser } from "./authActions";
import { User } from "next-auth";

export const useGetBidsByAuctionId = (auctionId: string) => {
    const [isLoading, setIsLoading] = useState(false);
    const [response, setResponse] = useState<Bid[] | undefined>(undefined);
  
    useEffect(() => {
        httpGet<Bid[]>(`${applicationUrls.gatewayUrl}/bids?auctionId=${auctionId}`, true).then((result) => {
            setResponse(result);
            setIsLoading(false);
          }).catch((error) => {
              setIsLoading(false);
              toast.error(error.message);
            });
    }, []);
  
    return { response, isLoading };
  };

export const useCreateBid = () => {
  const [response, setResponse] = useState<Bid | undefined>(undefined);
  const [isCreating, setIsCreating] = useState(false);

  const create = (bid: Bid) => {
    setIsCreating(true);
    const url = `${applicationUrls.gatewayUrl}/bids`;

    httpPost<Bid>(url, bid, true).then((r) => {
      invalidatePath('/');
      setIsCreating(false);
      setResponse(r);
      toast.success('Bid placed');
    }).catch((error) => {
        setIsCreating(false);
        toast.error(error.message);
      });;
  };

  return { create, isCreating, response };
};

export const useGetCurrentUser = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [currentUser, setCurrentUser] = useState<User | undefined>();

  useEffect(() => {
    setIsLoading(true);
    getCurrentUser().then(res => {
      setCurrentUser(res as User);
      setIsLoading(false);
    });
  }, []);

  return { currentUser, isLoading };
}
