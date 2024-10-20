import {PlatformConstants} from "./PlatformConstants";

export class PlatformService {
    private platform: string | undefined = undefined;

    isWeb() {
        return !this.isAndroid() && !this.isIOS();
    }

    isAndroid() {
        return typeof window.hybridWebViewHost != 'undefined';
    }

    isIOS() {
        return typeof window.webkit != 'undefined' && typeof window.webkit.messageHandlers != 'undefined' && typeof window.webkit.messageHandlers.webwindowinterop != 'undefined';
    }

    getPlatform(): string {
        return this.isAndroid() ? PlatformConstants.Android : this.isIOS() ? PlatformConstants.IOS : PlatformConstants.Web;
    }
}

export const platformService = new PlatformService();