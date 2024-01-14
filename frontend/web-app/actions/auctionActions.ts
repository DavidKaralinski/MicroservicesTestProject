'use server'

import { applicationUrls } from "@/common/appConfiguration";
import { getTokenWorkaround } from "./authActions";

export const get = async<T> (url: string) => {
    const res = await fetch(url);

    if(!res.ok) throw new Error(res.statusText);

    return await res.json() as T;
}

export const updateAuctionTest = async() => {
    const token = await getTokenWorkaround();
    console.log('token = ' + token?.access_token);

    const data = {
        make: 'Ford',
        model: 'GT',
        color: 'White',
        year: 2020,
        mileage: Math.floor(Math.random() * 100000) + 1
    };

    const res = await fetch(`${applicationUrls.gatewayUrl}/auctions/afbee524-5972-4075-8800-7d1f9d7b0a0c`, {
        method: 'PUT',
        headers: { 
            'Authorization': `Bearer ${token?.access_token}`,
            'Content-type': 'application/json'
         },
        body: JSON.stringify(data)
    });

    return { status: res.status, message: res.statusText };
}