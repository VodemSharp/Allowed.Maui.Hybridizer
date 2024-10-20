namespace Allowed.Maui.Hybridizer.Abstractions.Callers;

public interface IHwvJsCaller
{
    // void call()
    public Task Call(string methodName);
    public Task WrappedCall(string methodName);

    // T call()
    public Task<TResult?> Call<TResult>(string methodName)
        where TResult : class;

    public Task<TResult?> WrappedCall<TResult>(string methodName)
        where TResult : class;

    // void call(R)
    public Task Call<TBody>(string methodName, TBody body);
    public Task WrappedCall<TBody>(string methodName, TBody body);

    // T call(R)
    public Task<TResult?> Call<TBody, TResult>(string methodName, TBody body)
        where TResult : class;

    public Task<TResult?> WrappedCall<TBody, TResult>(string methodName, TBody body)
        where TResult : class;

    public void SetTaskResult(string message);
}