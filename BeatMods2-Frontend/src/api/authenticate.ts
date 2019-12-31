import { requestPostRoute } from "./index";

export interface IAuthenticateRequest {
    code: string;
}

export function request(req: IAuthenticateRequest): Promise<void> {
    return requestPostRoute("authenticate", req);
}

export function authenticate(code: string): Promise<void> {
    return request({ code });
}
