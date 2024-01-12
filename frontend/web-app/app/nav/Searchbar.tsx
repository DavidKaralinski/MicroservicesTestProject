'use client'

import React, { useEffect, useState } from 'react'
import { FaSearch } from 'react-icons/fa'
import { useSearchParamsStore } from '../../hooks/useSearchParamsStore'

export default function Searchbar() {
    const searchWord = useSearchParamsStore(state => state.searchValue);
    const setParams = useSearchParamsStore(state => state.setParams);
    const setSearchValue = useSearchParamsStore(state => state.setSearchValue);
    
    const [searchWordValue, setSearchWordValue] = useState<string | null>(null);

    useEffect(() => {
        setSearchWordValue(searchWord);
    }, [searchWord])

    const onKeyPressed = (event: any) => {
        if(event.key === 'Enter'){
            submitSearch();
        }
    }

    const submitSearch = () => {
        setSearchValue(searchWordValue);
    }

  return (
    <div className='flex w-[50%] items-center rounded-full border-2 py-2 shadow-sm'>
        <input 
            onChange={(e) => { setSearchWordValue(e.target.value); }}
            onKeyDown={onKeyPressed}
            value={searchWordValue || ''}
            type='text'
            placeholder='Search for cars by make'
            className='
                flex-grow
                pl-5
                bg-transparent
                border-transparent
                focus:border-transparent
                focus:outline-none
                focus:ring-0
                text-sm
                text-gray-500
            '
            />
        <button onClick={submitSearch}>
            <FaSearch size={34} 
                className='bg-gray-600 text-white rounded-full py-2 cursor-pointer mx-2' />
        </button>
    </div>
  )
}
