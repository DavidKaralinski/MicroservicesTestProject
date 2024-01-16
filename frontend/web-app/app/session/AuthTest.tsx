'use client'

import { updateAuctionTest, useUpdateAuction } from "@/actions/auctionActions";
import { Button } from "flowbite-react";
import React, { useState } from "react";

export default function AuthTest() {
  const [testResult, setTestResult] = useState<any>();
  const { update, isUpdating } = useUpdateAuction();

  const testData = {
    id: 'afbee524-5972-4075-8800-7d1f9d7b0a0c',
    make: 'Ford',
    model: 'GT',
    color: 'White',
    year: 2020,
    mileage: Math.floor(Math.random() * 100000) + 1
  };

  const testUpdate = () => {
    update(testData)
  };

  return (
    <div className="flex items-center gap-4">
      <Button outline isProcessing={isUpdating} onClick={testUpdate}>
        Test update
      </Button>
      {!isUpdating && !!testResult && <div>{'Test'}</div>}
    </div>
  );
}
