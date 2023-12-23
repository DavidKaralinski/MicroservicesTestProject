export const get = async (url: string) => {
    const res = await fetch(url);

    if(!res.ok) throw new Error(res.statusText);

    return res.json();
}