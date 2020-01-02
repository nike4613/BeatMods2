import { prepareRoutes, routes, requestGetRoute } from "../index";

export interface ILoginResponse {
    authTarget: string; // the full URL to the OAuth request for authentication
}

export interface ILoginRequest {
    success: string;
    failure?: string;
    userData?: string;
}

export async function request(req: ILoginRequest): Promise<ILoginResponse> {
    await prepareRoutes();
    return await requestGetRoute(routes().users.login, req);
}

export async function getGithubAuthUrl(req: ILoginRequest): Promise<string> {
    return (await request(req)).authTarget;
}
