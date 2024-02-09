"use client";

import { useAuctionsStore } from "@/hooks/useAuctionsStore";
import { useBidsStore } from "@/hooks/useBidsStore";
import { AcceptedBidStatusChangedNotification, Auction, AuctionDeletedNotification, AuctionFinishedNotification, Bid, BidPlacedNotification, BidStatus } from "@/types";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { User } from "next-auth";
import React, { ReactNode, useEffect, useState } from "react";
import toast from "react-hot-toast";
import AuctionCreatedToast from "../components/AuctionCreatedToast";
import AuctionFinishedToast from "../components/AuctionFinishedToast";

type Props = {
  children: ReactNode;
  user: Partial<User>;
};

export default function SignalRProvider({ children, user }: Props) {
  const [connection, setConnection] = useState<HubConnection | undefined>(
    undefined
  );

  const { setCurrentPrice, removeAuction, auctions } = useAuctionsStore();
  const { addBid, updateBidStatus } = useBidsStore();

  useEffect(() => {
    const url = process.env.NODE_ENV === 'production'
       ? 'https://api.testproject.com/notifications' 
       : process.env.NEXT_PUBLIC_NOTIFICATIONS_URL!;
       
    const newConnection = new HubConnectionBuilder()
      .withUrl(url)
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (!connection || !user) {
      return;
    }

    connection
      .start()
      .then(() => {
        console.log("Connected to signalR notifications");

        connection.on("BidPlaced", (bid: BidPlacedNotification) => {
          if(bid.bidStatus === BidStatus.Accepted || bid.bidStatus == BidStatus.AcceptedBelowReserve){
            setCurrentPrice(bid.auctionId, bid.bidAmount);
          }

          if(bid.bidderName !== user.name){
            addBid({ status: bid.bidStatus, amount: bid.bidAmount, id: bid.bidId, ...bid } as Bid);
          }
        });

        connection.on(
          "AuctionFinished",
          (notification: AuctionFinishedNotification) => {
            return toast(<AuctionFinishedToast notification={notification} />, 
                            {duration: 10000, icon: null})
          }
        );

        connection.on("AcceptedBidStatusChanged", (notification: AcceptedBidStatusChangedNotification) => {
            updateBidStatus(notification.bidId, notification.bidStatus);
        });

        connection.on("AuctionDeleted", (notification: AuctionDeletedNotification) => {
            removeAuction(notification.id);
        });

        connection.on("AuctionCreated", (auction: Auction) => {
            if(user.name !== auction.sellerName){
                return toast(<AuctionCreatedToast auction={auction} />, 
                                {duration: 10000})
            }
          });
      })
      .catch((err) => console.log(err));

    return () => {
      connection?.stop();
    };
  }, [connection, updateBidStatus, removeAuction, setCurrentPrice, addBid, user]);

  return children;
}
