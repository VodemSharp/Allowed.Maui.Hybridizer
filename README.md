# Allowed.Maui.Hybridizer

Allowed.Maui.Hybridizer is a library that provides a seamless bridge between JavaScript applications and .NET MAUI, enabling the development of reliable and efficient hybrid mobile applications. By combining the flexibility of web technologies like JavaScript and modern frameworks such as Svelte with the native capabilities of MAUI, developers can build dynamic cross-platform apps with ease.

|| Hybridizer                                                                                                               | Hybridizer.AutoGen                                                                                                                         | Hybridizer.Essentials                                                                                                                         |
|-|--------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------------------------------------------|
|*NuGet*| [![NuGet](https://img.shields.io/nuget/v/Allowed.Maui.Hybridizer)](https://www.nuget.org/packages/Allowed.Maui.Hybridizer) | [![NuGet](https://img.shields.io/nuget/v/Allowed.Maui.Hybridizer.AutoGen)](https://www.nuget.org/packages/Allowed.Maui.Hybridizer.AutoGen) | [![NuGet](https://img.shields.io/nuget/v/Allowed.Maui.Hybridizer.Essentials)](https://www.nuget.org/packages/Allowed.Maui.Hybridizer.Essentials) |

> Note: The framework was inspired by Capacitor but tailored to work within the .NET MAUI ecosystem for more efficient hybrid app development.

# Table of Contents

* [Why Allowed.Maui.Hybridizer?](#why-allowedmauihybridizer)
    * [Capacitor](#capacitor)
    * [Svelte Native (based on NativeScript)](#svelte-native-based-on-nativescript)
* [Getting Started](#getting-started)
* [Upgrading an Existing Project](#upgrading-an-existing-project)
    * [Add NuGet Packages](#add-nuget-packages)
    * [Modify MauiProgram.cs](#modify-mauiprogramcs)
    * [Update the Page with the HybridWebView](#update-the-page-with-the-hybridwebview)
    * [Add HybridWebView Handlers](#add-hybridwebview-handlers)
    * [Add the Client App Folder with JavaScript Application](#add-the-client-app-folder-with-javascript-application)
    * [Copy the Hybridizer and Plugin Interfaces](#copy-the-hybridizer-and-plugin-interfaces)
* [Creating a New Plugin](#creating-a-new-plugin)
  * [Create a .NET MAUI Class Library](#create-a-net-maui-class-library)
  * [Add the AutoGen NuGet Package](#add-the-autogen-nuget-package)
  * [Implement the C# Plugin](#implement-the-c-plugin)
  * [Create the JavaScript Interface](#create-the-javascript-interface)
  * [Register the Plugin with Dependency Injection](#register-the-plugin-with-dependency-injection)
* [Calling JavaScript from C# Code](#calling-javascript-from-c-code)
  * [Register a JavaScript Method](#register-a-javascript-method)
  * [Invoke JavaScript from C#](#invoke-javascript-from-c)
  * [Rebinding Methods (Optional)](#rebinding-methods-optional)
* [Backlog](#backlog)

# Why Allowed.Maui.Hybridizer?

In my search for a mobile framework that allows the use of JavaScript, particularly Svelte, I explored the available options. There were two main contenders:

### Capacitor
A great solution with broad platform support, but it has some challenges when it comes to plugin development. Writing new plugins requires extensive knowledge of Java and Swift, which can be a barrier for many developers.

### Svelte Native (based on NativeScript)
While promising, the support for this framework is limited and lacks the robustness needed for production-ready mobile applications.

# Getting Started

### 1. Install Allowed.Maui.Hybridizer.Templates:

```
dotnet new install Allowed.Maui.Hybridizer.Templates::1.0.0-beta
```

### 2. Create a new project
You can create the project using the command line or your preferred IDE like Visual Studio or Rider.

```
dotnet new maui-hybridizer
```

### 3. Build npm project:
Navigate to the `ClientApp` directory and install dependencies:

```
cd ClientApp
npm install
npm run build
```

### 4. Run project:
For <b>Android</b>:
```
dotnet build -t:Run -f net9.0-android
```
For <b>iOS</b>:
```
dotnet build -t:Run -f net9.0-ios
```

# Upgrading an Existing Project

To integrate `Allowed.Maui.Hybridizer` into an existing .NET MAUI project, follow these steps:

## Add NuGet Packages

Add the following packages to your project via the NuGet Package Manager:
* `Allowed.Maui.Hybridizer`
* `Allowed.Maui.Hybridizer.Essentials` (Optional)

## Modify MauiProgram.cs

Register the hybridizer services in your MAUI application. If you’re only using the core hybridizer:

```csharp
builder.Services.AddHybridizer();
```

If you want to include plugins from essentials:

```csharp
builder.Services.AddHybridizer(HwvPluginModule.Invoke);
builder.Services.RegisterAlertPlugin();
builder.Services.RegisterBatteryPlugin();
builder.Services.Register...
```

## Update the Page with the HybridWebView
In the XAML file of the page where you want to include the hybrid web view (e.g., `MainPage.xaml`), add the `HybridWebView` control:
```xaml
<Grid RowDefinitions="Auto,*"
      ColumnDefinitions="*">
    <HybridWebView x:Name="HybridWebView"
                   RawMessageReceived="OnHybridWebViewRawMessageReceived"
                   Grid.Row="1" />
</Grid>
```

## Add HybridWebView Handlers

In the code-behind of your page (e.g., `MainPage.xaml.cs`), implement the necessary handlers:
```csharp
public partial class MainPage : ContentPage
{
    private readonly HwvJsCaller _jsCaller;
    private readonly ILogger<MainPage> _logger;
    private readonly HwvPluginHandler _pluginHandler;

    public MainPage(HwvContext context, HwvJsCaller jsCaller, ILogger<MainPage> logger, HwvPluginHandler pluginHandler)
    {
        _jsCaller = jsCaller;
        _logger = logger;
        _pluginHandler = pluginHandler;

        InitializeComponent();

        context.Initialize(this, HybridWebView);
    }

    private async void OnHybridWebViewRawMessageReceived(object sender, HybridWebViewRawMessageReceivedEventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(e.Message)) return;

            var messageParts = e.Message.Split(new[] { '|' }, 2);
            var messageType = messageParts[0];
            var message = messageParts.Length > 1 ? messageParts[1] : string.Empty;

            switch (messageType)
            {
                case HwvMessageTypes.Plugin:
                    await _pluginHandler.HandleRawMessage(this, message);
                    break;
                case HwvMessageTypes.Call:
                    _jsCaller.SetTaskResult(message);
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing raw message");
        }
    }
}
```

## Add the Client App Folder with JavaScript Application
Create a client app folder containing your JavaScript application. For example, to set up a new Svelte application using Vite:

```
npm create vite@latest ClientApp
```
When prompted, select Svelte (TypeScript) as the framework.
\
\
Navigate to the new directory and install dependencies:
```
cd ClientApp
npm install
```
Set the build output directory to `./Resources/Raw/wwwroot` so that the built files are accessible to the MAUI application. In `vite.config.js`, add or modify the build configuration:
```javascript
import { defineConfig } from 'vite';
import { svelte } from '@sveltejs/vite-plugin-svelte';

export default defineConfig({
  plugins: [svelte()],
  build: {
    outDir: '../Resources/Raw/wwwroot',
  },
});
```
Now, build the project:
```
npm run build
```
This will output the built files to the Resources/Raw/wwwroot directory of your MAUI project.

## Copy the Hybridizer and Plugin Interfaces

Since the Hybridizer NPM package is not yet published, you need to manually copy the Hybridizer app code into your client application:

* <b>Copy the Hybridizer Code</b>: From the sample project in this repository, copy the contents of the hybridizer folder into your client application’s source code (e.g., inside the src directory).

* <b>Add Plugin Interfaces</b>: Copy the plugin interfaces from the plugins folder inside the `ClientApp` folder of the sample project into your client application. This includes any TypeScript interfaces or helper functions required for your plugins.

Your project structure should look something like this:

```
ClientApp/
├── src/
│   ├── hybridizer/
│   │   └── ... (Hybridizer app code)
│   ├── plugins/
│   │   └── ... (Plugin interfaces)
│   └── main.ts
├── package.json
├── vite.config.js
└── ...
```

# Creating a New Plugin

To extend the functionality of your application, you might want to create custom plugins. Here’s how you can create a new plugin and communicate with it from JavaScript:

## Create a .NET MAUI Class Library
Create a new .NET MAUI Class Library project. Ensure that the .NET version is less than 9, as Roslyn analyzers don’t support .NET 9 in all IDEs and CLIs.

```
dotnet new mauiclasslib -n MyCustomPlugin -f net8.0
```

## Add the AutoGen NuGet Package
Add the `Allowed.Maui.Hybridizer.AutoGen` NuGet package to your plugin project:

```
dotnet add package Allowed.Maui.Hybridizer.AutoGen
```

## Implement the C# Plugin
Create your plugin class by following the structure below. Replace the namespace with your own.

```csharp
using System.Text.Json.Serialization;
using Allowed.Maui.Hybridizer.Abstractions.Attributes;
using Allowed.Maui.Hybridizer.Abstractions.Plugins;

namespace MyCustomPlugin;

[HwvPlugin]
public class StoragePlugin
{
    [HwvMethod]
    public GetResponse Get([HwvPayload] GetRequest request)
    {
        return new GetResponse(Preferences.Default.Get(request.Key, string.Empty));
    }

    [HwvMethod]
    public void Set([HwvPayload] SetRequest request)
    {
        Preferences.Default.Set(request.Key, request.Value);
    }

    [HwvMethod]
    public void Remove([HwvPayload] RemoveRequest request)
    {
        Preferences.Default.Remove(request.Key);
    }

    public record GetRequest([property: JsonPropertyName("key")] string Key);

    public record GetResponse([property: JsonPropertyName("value")] string Value);

    public record SetRequest(
        [property: JsonPropertyName("key")] string Key,
        [property: JsonPropertyName("value")] string Value);

    public record RemoveRequest([property: JsonPropertyName("key")] string Key);
}
```

* <b>Explanation:</b>
  * Attributes:
    * `[HwvPlugin]` marks the class as a Hybridizer plugin.
    * `[HwvMethod]` marks the methods that can be called from JavaScript.
    * `[HwvPayload]` specifies the parameter that contains the payload from JavaScript.
  * Methods:
    * `Get`, `Set`, and `Remove` interact with `Preferences.Default` to store and retrieve data.

## Register the Plugin with Dependency Injection
After implementing your plugin, you need to register it with the MAUI dependency injection container so that it can be recognized by the Hybridizer.

In your `MauiProgram.cs`, add your plugin’s `HwvPluginModule.Invoke` method to the `AddHybridizer` call:
```csharp
builder.Services.AddHybridizer(Essentials.HwvPluginModule.Invoke, Plugins.HwvPluginModule.Invoke);
```
>Note: `HwvPluginModule` is an autogenerated class created by the `Allowed.Maui.Hybridizer.AutoGen` package when you build your plugin. It contains methods to register your plugin’s services.

## Create the JavaScript Interface
In your client application’s plugins folder, create a JavaScript file that defines the interface to communicate with your plugin:

```javascript
import { bridgeService } from "../hybridizer/BridgeService";
import { PlatformConstants } from "../hybridizer/PlatformConstants";
import { platformService } from "../hybridizer/PlatformService";

export class StoragePlugin {
  async get(key: string): Promise<GetResponse | undefined> {
    if (platformService.getPlatform() === PlatformConstants.Web) {
      return { value: localStorage.getItem(key) };
    } else {
      return await bridgeService.invoke<GetResponse>('Storage', 'Get', { key });
    }
  }

  async set(key: string, value: string): Promise<void> {
    if (platformService.getPlatform() === PlatformConstants.Web) {
      localStorage.setItem(key, value);
    } else {
      await bridgeService.invoke('Storage', 'Set', { key, value });
    }
  }

  async remove(key: string): Promise<void> {
    if (platformService.getPlatform() === PlatformConstants.Web) {
      localStorage.removeItem(key);
    } else {
      await bridgeService.invoke('Storage', 'Remove', { key });
    }
  }
}

export interface GetResponse {
  value?: string | null;
}

export const storagePlugin = new StoragePlugin();
```

* <b>Explanation:</b>
  * <b>Platform Check:</b>
    * The plugin checks if it’s running on the web or a native platform using `platformService.getPlatform()`.
  * <b>Methods:</b>
    * `get`, `set`, and `remove` methods interact with `localStorage` when on the web, or invoke the native plugin methods via bridgeService when on a native platform.
  * <b>Usage:</b>
    * Import `storagePlugin` wherever you need to use the storage functionality in your JavaScript code.

> Note: Ensure that the paths to BridgeService, PlatformConstants, and PlatformService are correct based on your project structure.

# Calling JavaScript from C# Code
`Allowed.Maui.Hybridizer` also enables you to call JavaScript functions directly from your C# code. This can be particularly useful when you need to trigger mobile application events like `Stop`, `Resume`, etc.

## Register a JavaScript Method
In your JavaScript code, use the `bridgeService.register` method to expose a function that can be called from C#:

```javascript
bridgeService.register('GetWebViewInfo', () => {
  return { userAgent: navigator.userAgent };
});
```
* Explanation:
  * The method `GetWebViewInfo` is registered and returns an object containing the `userAgent` string.
  * This method can now be invoked from the C# code.

## Invoke JavaScript from C#
In your C# code, inject an instance of `IHwvJsCaller` (or `HwvJsCaller` inside main project) and use it to call the JavaScript method:

```csharp
using System.Text.Json.Serialization;

// ...

public class GetWebViewInfo
{
    [JsonPropertyName("userAgent")]
    public string UserAgent { get; set; } = null!;
}

// ...

var webViewInfo = await _jsCaller.WrappedCall<GetWebViewInfo>("GetWebViewInfo");
```

* Explanation:
  * Call JavaScript Method: Use `WrappedCall<T>` to invoke the JavaScript method and receive a strongly-typed response.
  * Define Response Class: The `GetWebViewInfo` class defines the structure of the response expected from the JavaScript method.

## Rebinding Methods (Optional)
If you need to rebind or unregister the JavaScript method, you can do so using:

```javascript
bridgeService.unregister('GetWebViewInfo');
```

After unregistering, you can register the method again in a different context or with updated logic.

> Note: Rebinding methods can be useful when the JavaScript context changes, or if you need to refresh the method’s implementation.

# Backlog

* Create More Plugins in `Essentials`:
  * Expand the Allowed.Maui.Hybridizer.Essentials package by adding more plugins to cover additional native functionalities, such as camera access, geolocation, file system, and more.
* Add More Documentation:
  * Improve and expand the documentation to cover more use cases, detailed API references, troubleshooting guides, and best practices for using `Allowed.Maui.Hybridizer`.
  * Include more code examples and tutorials to help developers get up to speed quickly.
* Publish the Hybridizer NPM Package:
  * Publish the Hybridizer JavaScript code as an NPM package to simplify the setup process and allow developers to install it directly via NPM.
* Support for .NET 9:
  * Update the Roslyn analyzers and package dependencies to support .NET 9 and future versions as they become stable.
* Testing and Stability:
  * Increase test coverage for both the C# and JavaScript components to ensure stability and reliability.
* Community Contributions:
  * Encourage community involvement by accepting pull requests, feature suggestions, and bug reports.