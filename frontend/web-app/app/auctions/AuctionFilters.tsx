"use client";

import { Button, ButtonGroup } from "flowbite-react";
import React from "react";
import { AiOutlineClockCircle, AiOutlineSortAscending } from "react-icons/ai";
import { GiFinishLine, GiFlame } from "react-icons/gi";
import { BsFillStopCircleFill } from "react-icons/bs";
import { useSearchParamsStore } from "../hooks/useSearchParamsStore";

type AuctionFiltersProps = {
};

const pageSizes = [4, 8, 12];

const orderButtons = [
  {
    label: "Alphabetical",
    icon: AiOutlineSortAscending,
    value: "Make",
  },
  {
    label: "End date",
    icon: AiOutlineClockCircle,
    value: "AuctionEnd",
  },
  {
    label: "Created date",
    icon: BsFillStopCircleFill,
    value: "CreatedAt",
  },
];

const filterButtons = [
  {
    label: "Live auctions",
    icon: GiFlame,
    value: "Live",
  },
  {
    label: "Ending < 6h",
    icon: GiFinishLine,
    value: "EndingLessThan6",
  },
  {
    label: "Completed",
    icon: BsFillStopCircleFill,
    value: "Completed",
  },
];

export default function AuctionFilters({
}: AuctionFiltersProps) {
  const { pageSize, setParams, orderBy, filterBy } = useSearchParamsStore();

  return (
    <div className="flex justify-between items-center mb-4">
      <div>
        <span className="uppercase text-sm text-gray-500 mr-4">Filter by</span>
        <ButtonGroup>
          {filterButtons.map(({ label, icon: Icon, value }) => (
            <Button
              key={value}
              onClick={() => {}}
              color={value === filterBy ? "red" : "gray"}
            >
              <Icon className="mr-3 h-4 w-4" />
              {label}
            </Button>
          ))}
        </ButtonGroup>
      </div>
      <div>
        <span className="uppercase text-sm text-gray-500 mr-4">Order by</span>
        <ButtonGroup>
          {orderButtons.map(({ label, icon: Icon, value }) => (
            <Button
              key={value}
              onClick={() => { setParams({orderBy: value}); }}
              color={value === orderBy ? "red" : "gray"}
            >
              <Icon className="mr-3 h-4 w-4" />
              {label}
            </Button>
          ))}
        </ButtonGroup>
      </div>
      <div>
        <span className="uppercase text-sm text-gray-500 mr-4">Page size</span>
        <ButtonGroup>
          {pageSizes.map((value, index) => (
            <Button
              key={index}
              value={value}
              onClick={() => setParams({pageSize: value})}
              color={value === pageSize ? "red" : "gray"}
              className="ring-0 focus:ring-0"
            >
              {value}
            </Button>
          ))}
        </ButtonGroup>
      </div>
    </div>
  );
}
