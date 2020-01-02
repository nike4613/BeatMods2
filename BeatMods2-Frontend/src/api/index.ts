import { fetch_api } from "./request";

interface IRoutes {
    routes: string;
    users: {
        login: string;
        authenticate: string;
        current: string;
    };
}

let _routes: IRoutes | null = null;

export async function prepareRoutes(force: boolean = false): Promise<void> {
    if (_routes != null && !force) return;
    _routes = await fetch_api<IRoutes>("https://localhost:5001/api/routes"); // todo: do this, but not bad
}

export function routes(): IRoutes {
    return _routes!;
}

export async function requestGetRoute<T, U>(base: string, data: U): Promise<T> {
    // todo: figure out how to do error handling
    if (routes == null) {
        await prepareRoutes();
    }

    let url: string = base;
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
export async function requestPostRoute<T, U>(base: string, data: U): Promise<T> {
    if (routes == null) {
        await prepareRoutes();
    }

    return await fetch_api<T>(base, {
        mode: "cors",
        method: "POST",
        body: JSON.stringify(data),
    });
}

// todo: make this api nicer
