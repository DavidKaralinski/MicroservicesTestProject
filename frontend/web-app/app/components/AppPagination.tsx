'use client'

import { Pagination } from 'flowbite-react'
import React from 'react'
import { useSearchParamsStore } from '../../hooks/useSearchParamsStore'

type AppPaginationProps = {
}

export default function AppPagination({}: AppPaginationProps) {
  const {pageCount, pageNumber, setParams} = useSearchParamsStore();

  const onPageChange = (selectedNumber: number) => {
    setParams({pageNumber: selectedNumber});
  }

  return (
    <Pagination
        currentPage={pageNumber}
        totalPages={pageCount}
        onPageChange={onPageChange}
        layout='pagination'
        showIcons={true}
        className='text-blue-500 mb-5'
    />
  )
}
