import {bridgeService} from "../hybridizer/BridgeService";
import {PlatformConstants} from "../hybridizer/PlatformConstants";
import {platformService} from "../hybridizer/PlatformService";

export class StoragePlugin {
    async get(key: string): Promise<GetResponse | undefined> {
        if (platformService.getPlatform() == PlatformConstants.Web)
            return {value: localStorage.getItem(key)};
        else {
            return <GetResponse>await bridgeService.invoke('Storage', 'Get', {key: key});
        }
    }

    async set(key: string, value: string) {
        if (platformService.getPlatform() == PlatformConstants.Web)
            localStorage.setItem(key, value);
        else {
            await bridgeService.invoke('Storage', 'Set', {key: key, value: value});
        }
    }

    async remove(key: string) {
        if (platformService.getPlatform() == PlatformConstants.Web)
            localStorage.removeItem(key);
        else {
            await bridgeService.invoke('Storage', 'Remove', {key: key});
        }
    }
}

export class GetResponse {
    value?: string | null;
}

export const storagePlugin = new StoragePlugin();