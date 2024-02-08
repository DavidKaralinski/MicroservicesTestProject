'use server'

import { revalidatePath } from "next/cache";
import { getTokenWorkaround } from "./authActions";

const apiUrl = process.env.API_URL;

export const httpGet = async<T>(url: string, needsAuthorization?: boolean) => {
    const requestOptions = {
        headers: {
            'Content-type': 'application/json',
            'Authorization': needsAuthorization ? `Bearer ${(await getTokenWorkaround())?.access_token}` : 'None'
        },
        method: 'GET'
    };

    try{
        const res = await fetch(apiUrl + url, requestOptions);
        if(!res.ok){
            console.log(res.statusText);
            throw new Error(res.statusText);
        } 
        
        return await res.json() as T;
    }
    catch(err){
        console.log(err);
    }
}

export const httpPut = async(url: string, data: any, needsAuthorization?: boolean) => {
    const requestOptions = {
        headers: {
            'Content-type': 'application/json',
            'Authorization': needsAuthorization ? `Bearer ${(await getTokenWorkaround())?.access_token}` : 'None'
        },
        method: 'PUT',
        body: JSON.stringify(data)
    };

    console.log(requestOptions);

    const res = await fetch(apiUrl + url, requestOptions);
    if(!res.ok) throw new Error(res.statusText);

    return;
}

export const httpPost = async<T>(url: string, data: T, needsAuthorization?: boolean) => {
    const requestOptions = {
        headers: {
            'Content-type': 'application/json',
            'Authorization': needsAuthorization ? `Bearer ${(await getTokenWorkaround())?.access_token}` : 'None'
        },
        method: 'POST',
        body: JSON.stringify(data)
    };

    const res = await fetch(apiUrl + url, requestOptions);

    if(!res.ok) throw new Error(res.statusText);

    return (await res.json()) as T;
}

export const httpDelete = async(url: string, needsAuthorization?: boolean) => {
    const requestOptions = {
        headers: {
            'Content-type': 'application/json',
            'Authorization': needsAuthorization ? `Bearer ${(await getTokenWorkaround())?.access_token}` : 'None'
        },
        method: 'DELETE',
    };

    const res = await fetch(apiUrl + url, requestOptions);

    if(!res.ok) throw new Error(res.statusText);

    return;
}

export const invalidatePath = (path: string) => {
    revalidatePath(path);
}
