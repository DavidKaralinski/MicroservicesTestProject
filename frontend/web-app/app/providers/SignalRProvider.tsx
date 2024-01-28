"use client";

import { applicationUrls } from "@/common/appConfiguration";
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
    const newConnection = new HubConnectionBuilder()
      .withUrl(`${applicationUrls.gatewayUrl}/notifications`)
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, [applicationUrls.gatewayUrl]);

  useEffect(() => {
    if (!connection) {
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

          if(bid.bidderName !== user.username){
            addBid({ status: bid.bidStatus, amount: bid.bidAmount, ...bid } as Bid);
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
            if(user.username !== auction.sellerName){
                return toast(<AuctionCreatedToast auction={auction} />, 
                                {duration: 10000})
            }
          });
      })
      .catch((err) => console.log(err));

    return () => {
      connection?.stop();
    };
  }, [connection, updateBidStatus, removeAuction, setCurrentPrice]);

  return children;
}
