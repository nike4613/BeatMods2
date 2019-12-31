import { fetch_api } from "./request";

interface IRoutes {
    [name: string]: string;
    routes: string;
    login: string;
    authenticate: string;
}

let routes: IRoutes | null = null;

export async function prepareRoutes(): Promise<void> {
    routes = await fetch_api<IRoutes>("https://localhost:5001/api/routes"); // todo: do this, but not bad
}

export function getRoute(name: string): string {
    return routes![name];
}

export async function requestGetRoute<T, U>(name: string, data: U): Promise<T> {
    // todo: figure out how to do error handling
    if (routes == null) {
        await prepareRoutes();
    }

    const urlBase: string = getRoute(name);
    let url: string = urlBase;
    if (Object.keys(data).length > 0) {
        // if there are any args
        const params: URLSearchParams = new URLSearchParams();
        for (let [key, value] of Object.entries(data)) {
            params.append(key, value.toString());
        }
        url += "?" + params.toString();
    }
    return await fetch_api<T>(url);
}

// i really don't like what eslint wants
// eslint-disable-next-line
export async function requestPostRoute<T, U>(name: string, data: U): Promise<T> {
    if (routes == null) {
        await prepareRoutes();
    }

    const urlBase: string = getRoute(name);
    return await fetch_api<T>(urlBase, {
        mode: "cors",
        method: "POST",
        body: JSON.stringify(data),
    });
}

// todo: make this api nicer
