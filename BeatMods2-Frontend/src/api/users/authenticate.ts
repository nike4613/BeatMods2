import { routes, requestPostRoute, prepareRoutes } from "../index";

export interface IAuthenticateRequest {
    code: string;
}

export interface IAuthenticateResponse {
    token: string;
    isNewUser: boolean;
}

export async function request(
    req: IAuthenticateRequest
): Promise<IAuthenticateResponse> {
    await prepareRoutes();
    return await requestPostRoute(routes().users.authenticate, req);
}

export function authenticate(code: string): Promise<IAuthenticateResponse> {
    return request({ code });
}
