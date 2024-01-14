'use client'

import { create } from "zustand";

type State = {
    pageNumber: number
    pageSize: number
    pageCount: number
    searchTerm: string | null
    searchValue: string | null
    orderBy: string
    filterBy: string
    seller?: string
    winner?: string
}

type Actions = {
    setParams: (params: Partial<State>) => void
    reset: () => void
    setSearchValue: (value: string | null) => void
}

const initialState: State = {
    pageNumber: 1,
    pageSize: 4,
    pageCount: 1,
    searchTerm: '',
    searchValue: null,
    orderBy: 'make',
    filterBy: 'live',
    seller: undefined,
    winner: undefined,
}

export const useSearchParamsStore = create<State & Actions>()((set) => ({
    ...initialState,

    setParams: (newParams: Partial<State>) => {
        set((state) => {
            if(newParams.seller){
                return {...state, ...newParams, winner: undefined}
            }

            if(newParams.winner){
                return {...state, ...newParams, seller: undefined}
            }

            if(newParams.pageCount && (state.pageNumber > newParams.pageCount)){
                return {...state, ...newParams, pageNumber: newParams.pageCount}
            }

            return {...state, ...newParams}
        })
    },

    reset: () => set(initialState),

    setSearchValue: (value: string | null) => {
        set({searchTerm: value})
    }
}))