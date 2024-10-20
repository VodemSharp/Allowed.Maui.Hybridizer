import './app.css'
import App from './App.svelte'
import type {Chrome, HybridWebViewHost, WebKit} from "./hybridizer/WindowInterfaces";
import {bridgeService} from "./hybridizer/BridgeService";

declare global {
    interface Window {
        hybridWebViewHost: HybridWebViewHost;
        webkit?: WebKit;
        chrome: Chrome,
        // @ts-ignore
        external: External
    }
}

bridgeService.initialize();

const app = new App({
    target: document.getElementById('app')!,
})

export default app
