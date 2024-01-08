'use client'

import { Pagination } from 'flowbite-react'
import React from 'react'

type AppPaginationProps = {
    currentPage: number;
    totalPages: number;
    onPageChange: (pageNumber: number) => void;
}

export default function AppPagination({currentPage, totalPages, onPageChange}: AppPaginationProps) {
  return (
    <Pagination
        currentPage={currentPage}
        totalPages={totalPages}
        onPageChange={onPageChange}
        layout='pagination'
        showIcons={true}
        className='text-blue-500 mb-5'
    />
  )
}
