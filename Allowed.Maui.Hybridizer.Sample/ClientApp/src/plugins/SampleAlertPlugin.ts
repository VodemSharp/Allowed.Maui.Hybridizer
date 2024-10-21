import {bridgeService} from "../hybridizer/BridgeService";
import {PlatformConstants} from "../hybridizer/PlatformConstants";
import {platformService} from "../hybridizer/PlatformService";

export class SampleAlertPlugin {
    async show() {
        if (platformService.getPlatform() == PlatformConstants.Web)
            alert('Sample alert');
        else {
            await bridgeService.invoke('SampleAlert', 'ShowAlert');
        }
    }
}

export const sampleAlertPlugin = new SampleAlertPlugin();