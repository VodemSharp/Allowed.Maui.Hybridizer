export interface HybridWebViewHost {
    sendMessage: (value: string) => void;
}

export interface WebKit {
    // Define the properties and methods you need here
    // For example:
    messageHandlers?: {
        webwindowinterop?: {
            postMessage: (message: any) => void;
        };
    };
}

export interface Chrome {
    webview: any;
}

export interface External {
    receiveMessage: (value: string) => void;
}