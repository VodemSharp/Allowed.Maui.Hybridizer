import {bridgeService} from "../hybridizer/BridgeService";
import {PlatformConstants} from "../hybridizer/PlatformConstants";
import {platformService} from "../hybridizer/PlatformService";

export class BatteryPlugin {
    async getInfo(): Promise<BatteryInfo | undefined> {
        if (platformService.getPlatform() != PlatformConstants.Web)
            return <BatteryInfo>await bridgeService.invoke('Battery', 'GetInfo');
        
        return undefined;
    }
}

export class BatteryInfo {
    state!: number;
    chargeLevel!: number;
}

export const batteryPlugin = new BatteryPlugin();