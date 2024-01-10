'use client'

import React from "react";
import { SiStudyverse } from 'react-icons/si';
import { useSearchParamsStore } from "../hooks/useSearchParamsStore";

export default function Logo() {
  const { reset } = useSearchParamsStore();

  return (
    <div onClick={reset} className="flex items-center gap-2 text-3xl font-semibold text-gray-600">
      <SiStudyverse size={34} />
      <div>Test Project</div>
    </div>
  );
}
