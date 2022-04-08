import { ISpaConfiguration } from "../Configuration/ConfigurationModel";

export default class HttpClient {
    public static getInstance(): HttpClient {
        if (!HttpClient.instance) {
            HttpClient.instance = new HttpClient();
        }
        return HttpClient.instance;
    }
    private static instance: HttpClient;
    private baseUrl: string;

    public initialize(configuration: ISpaConfiguration): void {
        this.baseUrl = configuration.apiBaseUrl;
    }

    public async getAsync<T>(controller: string, action: string, params?: any): Promise<T> {
        const paramsAsString: string = this.encodeParams(params);
        const response = await this.rawRequest(this.baseUrl + "/" + controller + "/" + action + paramsAsString, HttpMethod.Get, null);
        if (!response.ok) {
            return null!;
        }

        const text = await response.text();
        if (text === "" || text == undefined) {
            return null!;
        }

        const data = JSON.parse(text);
        return <T>data;
    }

    public async postSingleRequestAsync(controller: string, action: string, params: any): Promise<any> {
        const response = await this.rawRequest(this.baseUrl + "/" + controller + "/" + action, HttpMethod.Post, params);
        if (!response.ok) {
            return undefined;
        }

        const text = await response.text();
        if (text === "" || text == undefined) {
            return null;
        }

        const data = JSON.parse(text);
        return data;
    }

    private async rawRequest(url: string, method: HttpMethod, params?: any): Promise<Response> {
        const body = params == undefined ? undefined : JSON.stringify(params);

        const headers = {
            "Content-Type": "application/json",
            "Accept": "application/json",
            "Access-Control-Allow-Origin": "*"
        };

        const response = await fetch(url, {
            headers,
            method,
            body
        });

        if (!response.ok) {
            alert(response.statusText);
            throw new Error(response.statusText);
        }
        return response;
    }

    private encodeParams(params: any): string {
        if (params == undefined) {
            return "";
        }

        const esc = encodeURIComponent;
        const query = Object.keys(params)
            .map((k) => esc(k) + "=" + esc(params[k]))
            .join("&");
        const encodedParams = "?" + query;
        return encodedParams;
    }
}

export enum HttpMethod {
    Get = "GET",
    Post = "POST",
    Put = "PUT",
    Delete = "DELETE"
}
