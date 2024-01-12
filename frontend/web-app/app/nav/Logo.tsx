'use client'

import React from "react";
import { SiStudyverse } from 'react-icons/si';
import { useSearchParamsStore } from "../../hooks/useSearchParamsStore";
import { usePathname, useRouter } from "next/navigation";

export default function Logo() {
  const router = useRouter();
  const pathName = usePathname();
  const { reset } = useSearchParamsStore();

  const onLogoClick = () => {
    if(pathName !== '/') router.push('/');
    reset();
  }

  return (
    <div onClick={onLogoClick} className="flex items-center gap-2 text-3xl font-semibold text-gray-600 cursor-pointer">
      <SiStudyverse size={34} />
      <div>Test Project</div>
    </div>
  );
}
