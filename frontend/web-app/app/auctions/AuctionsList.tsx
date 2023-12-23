import { get } from "../services/HttpRequestService";

export const AuctionsList = async () => {
    const response = await get('https://swapi.dev/api/people/1');

    return (
        <div>{JSON.stringify(response)}</div>
    )
}