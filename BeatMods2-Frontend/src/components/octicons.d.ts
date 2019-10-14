declare module "@primer/octicons" {
    export interface Icon {
        name: string;
        width: number;
        height: number;
        path: string;
        symbol: string;
        options: { [key: string]: any };
        toSVG(opts: { [key: string]: any }): string;
    }
    export interface Icons {
        [name: string]: Icon | undefined;
    }
    const data: Icons;
    export default data;
}
