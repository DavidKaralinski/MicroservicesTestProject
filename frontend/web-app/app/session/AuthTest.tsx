"use client";

import { updateAuctionTest } from "@/actions/auctionActions";
import { Button } from "flowbite-react";
import React, { useState } from "react";

export default function AuthTest() {
  const [testResult, setTestResult] = useState<any>();
  const [isLoading, setIsLoading] = useState(false);

  const testUpdate = () => {
    setIsLoading(true);
    updateAuctionTest().then((res) => {
      setTestResult(res);
      setIsLoading(false);
    });
  };

  return (
    <div className="flex items-center gap-4">
      <Button outline isProcessing={isLoading} onClick={testUpdate}>
        Test update
      </Button>
      {!isLoading && !!testResult && <div>{JSON.stringify(testResult)}</div>}
    </div>
  );
}
