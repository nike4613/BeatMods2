import { prepareRoutes, routes, requestGetRoute } from "../index";

export interface ICurrentUser {
    name: string;
    id: string; // GUID
    created: Date;
    profile: string;
    groups: [string]; // [GUID] of group GUIDs
}
export interface ICurrentUserGitHub extends ICurrentUser {
    githubName: string; // will usually be the same as name
}

interface ICurrentResponse {
    name: string;
    id: string;
    created: string;
    profile: string;
    groups: [string];
    githubName?: string;
}

export async function getCurrent(
    includeGithubInfo: boolean = false
): Promise<ICurrentUser | ICurrentUserGitHub> {
    await prepareRoutes();
    let resp = (await requestGetRoute(routes().users.current, {
        includeGithubInfo,
    })) as ICurrentResponse;
    return {
        ...resp,
        created: new Date(resp.created),
        githubName: resp.githubName,
    };
}

export function getCurrentGithubInfo(): Promise<ICurrentUserGitHub> {
    return getCurrent(true) as Promise<ICurrentUserGitHub>;
}
