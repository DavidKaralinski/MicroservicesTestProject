export const get = async<T> (url: string) => {
    const res = await fetch(url);

    if(!res.ok) throw new Error(res.statusText);

    return await res.json() as T;
}