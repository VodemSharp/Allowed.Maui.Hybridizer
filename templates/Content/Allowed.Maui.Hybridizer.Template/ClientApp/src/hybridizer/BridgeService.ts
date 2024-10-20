import type {External} from "./WindowInterfaces";

export class BridgeService {
    taskResolvers: Record<number, (value: object | undefined) => void> = {};
    methods: Record<string, (value: object | undefined) => Promise<any> | any> = {};

    initialize() {
        function DispatchHybridWebViewMessage(message: string) {
            const event = new CustomEvent("HybridWebViewMessageReceived", {detail: {message: message}});
            window.dispatchEvent(event);
        }

        if (window.chrome && window.chrome.webview) {
            // Windows WebView2
            window.chrome.webview.addEventListener('message', (arg: any) => {
                DispatchHybridWebViewMessage(arg.data);
            });
        } else if (window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.webwindowinterop) {
            // @ts-ignore iOS and MacCatalyst WKWebView
            (<External>window.external) = {
                receiveMessage: (message: string) => {
                    DispatchHybridWebViewMessage(message);
                }
            };
        } else {
            // Android WebView
            window.addEventListener('message', arg => {
                DispatchHybridWebViewMessage(arg.data);
            });
        }

        window.addEventListener("HybridWebViewMessageReceived", async (e: any) => {
            const messageParts = e.detail.message.split('|');
            const messageType = messageParts[0];
            const message = messageParts.length > 1 ? messageParts.slice(1).join('|') : '';

            if (messageType == BridgeTypes.Plugin) {
                const response: PluginMethodResponse = JSON.parse(message);
                this.taskResolvers[response.taskId](response.payload);
            } else if (messageType == BridgeTypes.Call) {
                const request: CallMethodRequest = JSON.parse(message);
                let response: object | undefined = undefined;

                if (this.methods[request.method]) {
                    const result = this.methods[request.method](request.payload);
                    response = result instanceof Promise ? await result : result;
                }

                this.callback(request.taskId, response);
            }
        });
    }
    
    private sendMessage(message: string) {
        this.sendMessageInternal('RawMessage', message);
    }

    private sendMessageInternal(type: string, message: string) {
        const messageToSend = `${type}|${message}`;

        if (window.chrome && window.chrome.webview) {
            // Windows WebView2
            window.chrome.webview.postMessage(messageToSend);
        } else if (window.webkit && window.webkit.messageHandlers && window.webkit.messageHandlers.webwindowinterop) {
            // iOS and MacCatalyst WKWebView
            window.webkit.messageHandlers.webwindowinterop.postMessage(messageToSend);
        } else {
            // Android WebView
            window.hybridWebViewHost.sendMessage(messageToSend);
        }
    }
    
    // Plugins
    async invoke(plugin: string, method: string, payload: object | undefined = undefined): Promise<object | undefined> {
        return await this.invokeRequest(new PluginMethodRequest({
            plugin: plugin,
            method: method,
            payload: payload
        }));
    }

    private async invokeRequest(request: PluginMethodRequest): Promise<object | undefined> {
        return new Promise((resolve, _) => {
            this.taskResolvers[request.taskId] = resolve;
            this.sendMessage(`${BridgeTypes.Plugin}|${JSON.stringify(request)}`);
        });
    }
    
    // Calls
    register(method: string, action: (value: any) => Promise<any> | any) {
        this.methods[method] = action;
    }

    unregister(method: string) {
        const newMethods: Record<string, (value: object | undefined) => Promise<any> | any> = {};

        for (let key in this.methods) {
            if (key !== method)
                newMethods[key] = this.methods[key];
        }

        this.methods = newMethods;
    }

    private callback(taskId: number, payload: object | undefined = undefined) {
        this.sendMessage(`${BridgeTypes.Call}|${JSON.stringify(new CallMethodResponse({
            taskId: taskId,
            payload: payload
        }))}`);
    }
}

export enum BridgeTypes {
    Plugin = 'Plugin',
    Call = 'Call'
}

let taskCounter: number = 0;

export class PluginMethodRequest {
    plugin!: string;
    method!: string;
    taskId: number;
    payload: object | undefined;
    
    public constructor(init?: Partial<PluginMethodRequest>) {
        Object.assign(this, init);
        this.taskId = taskCounter++;
    }
}

export class PluginMethodResponse {
    taskId!: number;
    payload: object | undefined;
}

export class CallMethodRequest {
    taskId!: number;
    method!: string;
    payload: object | undefined;
}

export class CallMethodResponse {
    taskId!: number;
    payload: object | undefined;

    public constructor(init?: Partial<CallMethodResponse>) {
        Object.assign(this, init);
    }
}

export const bridgeService = new BridgeService();