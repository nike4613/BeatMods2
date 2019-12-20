export function fetch_api<T>(
    request: RequestInfo,
    init?: RequestInit | undefined
): Promise<T> {
    return new Promise((resolve) => {
        fetch(request, init)
            .then((resp) => resp.json())
            .then((body) => resolve(body));
    });
}
