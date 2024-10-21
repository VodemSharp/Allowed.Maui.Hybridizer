import './app.css'
import App from './App.svelte'
import {bridgeService} from "./hybridizer/BridgeService";
import {platformService} from "./hybridizer/PlatformService";

if (!platformService.isWeb())
  bridgeService.initialize();

const app = new App({
  target: document.getElementById('app')!,
})

export default app
