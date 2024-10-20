import './app.css'
import App from './App.svelte'
import {bridgeService} from "./hybridizer/BridgeService";

bridgeService.initialize();

const app = new App({
  target: document.getElementById('app')!,
})

export default app
