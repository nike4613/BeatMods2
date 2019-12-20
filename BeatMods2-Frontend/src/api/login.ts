import { requestGetRoute } from "./index";

export interface ILoginResponse {
    authTarget: string; // the full URL to the OAuth request for authentication
}

export interface ILoginRequest {
    returnTo: string;
    userData?: string;
}

export function request(req: ILoginRequest): Promise<ILoginResponse> {
    return requestGetRoute("login", req);
}

export async function getGithubAuthUrl(req: ILoginRequest): Promise<string> {
    return (await request(req)).authTarget;
}
