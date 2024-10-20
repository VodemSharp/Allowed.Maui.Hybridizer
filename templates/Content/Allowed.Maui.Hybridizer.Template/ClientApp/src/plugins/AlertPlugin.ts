import {bridgeService} from "../hybridizer/BridgeService";
import {PlatformConstants} from "../hybridizer/PlatformConstants";
import {platformService} from "../hybridizer/PlatformService";

export class AlertPlugin {
    async show(title: string, message: string, accept: string, cancel: string, flowDirection: number) {
        if (platformService.getPlatform() == PlatformConstants.Web)
            alert(message);
        else {
            await bridgeService.invoke('Alert', 'Show', {
                title: title,
                message: message,
                accept: accept,
                cancel: cancel,
                flowDirection: flowDirection
            });
        }
    }
}

export const alertPlugin = new AlertPlugin();