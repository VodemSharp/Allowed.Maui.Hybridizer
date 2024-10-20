using System.Collections.Concurrent;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Allowed.Maui.Hybridizer.Abstractions.Callers;
using Allowed.Maui.Hybridizer.Contexts;
using Allowed.Maui.Hybridizer.Messages;
using Microsoft.Maui;
using Microsoft.Maui.Dispatching;

namespace Allowed.Maui.Hybridizer.Callers;

public class HwvJsCaller(HwvContext context) : IHwvJsCaller
{
    private readonly object _lockerObject = new();
    private readonly ConcurrentDictionary<long, TaskCompletionSource<JsonElement?>> _tasks = new();
    private long _taskCounter;

    // void call()
    public async Task Call(string methodName)
    {
        var taskId = GetTaskId();

        context.HybridWebView.SendRawMessage(AddCallPrefix(JsonSerializer.Serialize(new HwvCall
            { TaskId = taskId, Method = methodName })));

        await GetTaskResult(taskId);
    }

    public async Task WrappedCall(string methodName)
    {
        var taskId = GetTaskId();

        context.Page.Dispatcher.Dispatch(() =>
        {
            context.HybridWebView.SendRawMessage(AddCallPrefix(JsonSerializer.Serialize(new HwvCall
                { TaskId = taskId, Method = methodName })));
        });

        await GetTaskResult(taskId);
    }

    // T call()
    public async Task<TResult?> Call<TResult>(string methodName)
        where TResult : class
    {
        var taskId = GetTaskId();

        context.HybridWebView.SendRawMessage(AddCallPrefix(JsonSerializer.Serialize(new HwvCall
            { TaskId = taskId, Method = methodName })));

        var result = await GetTaskResult(taskId);

        return result?.Deserialize<TResult>();
    }

    public async Task<TResult?> WrappedCall<TResult>(string methodName)
        where TResult : class
    {
        return await context.Page.Dispatcher.DispatchAsync(async () => await Call<TResult>(methodName));
    }

    // void call(R)
    public async Task Call<TPayload>(string methodName, TPayload payload)
    {
        var taskId = GetTaskId();

        context.HybridWebView.SendRawMessage(AddCallPrefix(JsonSerializer.Serialize(new HwvCall<TPayload>
            { TaskId = taskId, Method = methodName, Payload = payload })));

        await GetTaskResult(taskId);
    }

    public async Task WrappedCall<TPayload>(string methodName, TPayload payload)
    {
        await context.Page.Dispatcher.DispatchAsync(async () => await Call(methodName, payload));
    }

    // T call(R)
    public async Task<TResult?> Call<TPayload, TResult>(string methodName, TPayload payload)
        where TResult : class
    {
        var taskId = GetTaskId();

        context.HybridWebView.SendRawMessage(AddCallPrefix(JsonSerializer.Serialize(new HwvCall<TPayload>
            { TaskId = taskId, Method = methodName, Payload = payload })));

        var result = await GetTaskResult(taskId);
        return result?.Deserialize<TResult>();
    }

    public async Task<TResult?> WrappedCall<TPayload, TResult>(string methodName, TPayload payload)
        where TResult : class
    {
        return await context.Page.Dispatcher.DispatchAsync(async () =>
            await Call<TPayload, TResult>(methodName, payload));
    }

    public void SetTaskResult(string message)
    {
        var request = JsonSerializer.Deserialize<HwvCallback>(message);
        if (request == null) throw new SerializationException();

        SetTaskResult(request.TaskId, request.Payload);
    }

    private long GetTaskId()
    {
        long taskId;
        lock (_lockerObject)
        {
            taskId = _taskCounter++;
        }

        return taskId;
    }

    private void SetTaskResult(long taskId, JsonElement? result)
    {
        if (_tasks.TryGetValue(taskId, out var tcs))
        {
            tcs.SetResult(result);
        }
        else
        {
            var tcsNew = new TaskCompletionSource<JsonElement?>();
            tcsNew.SetResult(result);
            _tasks[taskId] = tcsNew;
        }
    }

    private async Task<JsonElement?> GetTaskResult(long taskId)
    {
        var tcs = _tasks.GetOrAdd(taskId, _ => new TaskCompletionSource<JsonElement?>());
        return await tcs.Task.ConfigureAwait(false);
    }

    private static string AddCallPrefix(string response)
    {
        return $"{HwvMessageTypes.Call}|{response}";
    }
}

public class HwvCall
{
    [JsonPropertyName("taskId")] public long TaskId { get; set; }
    [JsonPropertyName("method")] public string Method { get; set; } = null!;
}

public class HwvCall<T> : HwvCall
{
    [JsonPropertyName("payload")] public T? Payload { get; set; }
}

public class HwvCallback
{
    [JsonPropertyName("taskId")] public long TaskId { get; set; }
    [JsonPropertyName("payload")] public JsonElement? Payload { get; set; }
}