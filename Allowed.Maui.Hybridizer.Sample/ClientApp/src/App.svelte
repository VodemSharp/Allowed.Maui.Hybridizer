<script lang="ts">
    import svelteLogo from './assets/svelte.svg'
    import viteLogo from '/vite.svg'
    import Counter from './lib/Counter.svelte'
    import {alertPlugin} from "./plugins/AlertPlugin";
    import {bridgeService} from "./hybridizer/BridgeService";
    import {onDestroy, onMount} from "svelte";
    import {clientLifetimePlugin} from "./plugins/ClientLifetimePlugin";
    import {type BatteryInfo, batteryPlugin} from "./plugins/BatteryPlugin";
    import {storagePlugin} from "./plugins/StoragePlugin";
    import {sampleAlertPlugin} from "./plugins/SampleAlertPlugin";

    let batteryText: string = 'Loading...';

    function setBatteryText(batteryInfo: BatteryInfo) {
        let stateText = 'Battery state could not be determined.';
        switch (batteryInfo.state) {
            case 1:
                stateText = 'Battery is actively being charged by a power source.';
                break;
            case 2:
                stateText = 'Battery is not plugged in and discharging.';
                break;
            case 3:
                stateText = 'Battery is full.';
                break;
            case 4:
                stateText = 'Battery is not charging or discharging, but in an in-between state.';
                break;
            case 5:
                stateText = 'Battery does not exist on the device.';
                break;
        }

        batteryText = `${stateText} ${batteryInfo.chargeLevel * 100}%!!!`;
    }

    onMount(async () => {
        await clientLifetimePlugin.initialize();

        const batteryInfo = await batteryPlugin.getInfo();
        setBatteryText(batteryInfo!);

        bridgeService.register('Resumed', () => {
            console.log('Hello World!');
        });

        bridgeService.register('Stopped', () => {
            console.log('Goodbye World!');
        });

        bridgeService.register('GetWebViewInfo', () => {
            return {userAgent: navigator.userAgent};
        });

        bridgeService.register('BatteryInfoChanged', (batteryInfo: BatteryInfo) => {
            setBatteryText(batteryInfo);
        });
        
        await storagePlugin.set('TestKey', 'TestValue');
        console.log((await storagePlugin.get('TestKey'))!.value);
        await storagePlugin.remove('TestKey');
        console.log((await storagePlugin.get('TestKey'))!.value);
    });

    onDestroy(() => {
        bridgeService.unregister('Resumed');
        bridgeService.unregister('Stopped');
        bridgeService.unregister('GetWebViewInfo');
        bridgeService.unregister('BatteryInfoChanged');

        clientLifetimePlugin.dispose();
    });
</script>

<main>
    <div>
        <a href="https://vitejs.dev" target="_blank" rel="noreferrer">
            <img src={viteLogo} class="logo" alt="Vite Logo"/>
        </a>
        <a href="https://svelte.dev" target="_blank" rel="noreferrer">
            <img src={svelteLogo} class="logo svelte" alt="Svelte Logo"/>
        </a>
    </div>
    <h1>Vite + Svelte</h1>

    <div class="card">
        <p>
            {batteryText}
        </p>
        <Counter/>
        <hr/>
        <button on:click={() => alertPlugin.show('Title', 'Message', 'Accept', 'Cancel', 2)}>
            show alert
        </button>
        <hr/>
        <button on:click={() => sampleAlertPlugin.show()}>
            show custom alert
        </button>
    </div>

    <p>
        Check out <a href="https://github.com/sveltejs/kit#readme" target="_blank" rel="noreferrer">SvelteKit</a>, the
        official Svelte app framework powered by Vite!
    </p>

    <p class="read-the-docs">
        Click on the Vite and Svelte logos to learn more
    </p>
</main>

<style>
    .logo {
        height: 6em;
        padding: 1.5em;
        will-change: filter;
        transition: filter 300ms;
    }

    .logo:hover {
        filter: drop-shadow(0 0 2em #646cffaa);
    }

    .logo.svelte:hover {
        filter: drop-shadow(0 0 2em #ff3e00aa);
    }

    .read-the-docs {
        color: #888;
    }
</style>
