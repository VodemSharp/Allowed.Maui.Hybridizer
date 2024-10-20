import {bridgeService} from "../hybridizer/BridgeService";
import {PlatformConstants} from "../hybridizer/PlatformConstants";
import {platformService} from "../hybridizer/PlatformService";

export class ClientLifetimePlugin {
    async initialize() {
        if (platformService.getPlatform() != PlatformConstants.Web)
            await bridgeService.invoke('ClientLifetime', 'Initialize');
    }

    async dispose() {
        if (platformService.getPlatform() != PlatformConstants.Web)
            await bridgeService.invoke('ClientLifetime', 'Dispose');
    }
}

export const clientLifetimePlugin = new ClientLifetimePlugin();