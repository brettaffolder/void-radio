using Microsoft.UI.Dispatching;

namespace VoidRadio.WinUI.Extensions;

public static class DispatcherQueueExtensions
{
    public static Task EnqueueAsync(this DispatcherQueue dispatcher, Action function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        if (dispatcher.HasThreadAccess)
        {
            try
            {
                function();

                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        }

        var tcs = new TaskCompletionSource<object?>();

        if (!dispatcher.TryEnqueue(priority, () =>
        {
            try
            {
                function();
                tcs.SetResult(null);
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }))
        {
            tcs.SetException(new InvalidOperationException("Failed to enqueue function."));
        }

        return tcs.Task;
    }

    public static Task<T> EnqueueAsync<T>(this DispatcherQueue dispatcher, Func<T> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        if (dispatcher.HasThreadAccess)
        {
            try
            {
                return Task.FromResult(function());
            }
            catch (Exception e)
            {
                return Task.FromException<T>(e);
            }
        }

        var tcs = new TaskCompletionSource<T>();

        if (!dispatcher.TryEnqueue(priority, () =>
        {
            try
            {
                tcs.SetResult(function());
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }))
        {
            tcs.SetException(new InvalidOperationException("Failed to enqueue function."));
        }

        return tcs.Task;
    }

    public static Task EnqueueAsync(this DispatcherQueue dispatcher, Func<Task> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        if (dispatcher.HasThreadAccess)
        {
            try
            {
                if (function() is Task result)
                {
                    return result;
                }

                return Task.FromException(new InvalidOperationException("Failed to enqueue function."));
            }
            catch (Exception e)
            {
                return Task.FromException(e);
            }
        }

        var tcs = new TaskCompletionSource<object?>();

        if (!dispatcher.TryEnqueue(priority, async () =>
        {
            try
            {
                if (function() is Task result)
                {
                    await result.ConfigureAwait(false);
                    tcs.SetResult(null);
                }
                else
                {
                    tcs.SetException(new InvalidOperationException("Failed to enqueue function."));
                }
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }))
        {
            tcs.SetException(new InvalidOperationException("Failed to enqueue function."));
        }

        return tcs.Task;
    }

    public static Task<T> EnqueueAsync<T>(this DispatcherQueue dispatcher, Func<Task<T>> function, DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
    {
        if (dispatcher.HasThreadAccess)
        {
            try
            {
                if (function() is Task<T> result)
                {
                    return result;
                }

                return Task.FromException<T>(new InvalidOperationException("Failed to enqueue function."));
            }
            catch (Exception e)
            {
                return Task.FromException<T>(e);
            }
        }

        var tcs = new TaskCompletionSource<T>();

        if (!dispatcher.TryEnqueue(priority, async () =>
        {
            try
            {
                if (function() is Task<T> result)
                {
                    var value = await result.ConfigureAwait(false);
                    tcs.SetResult(value);
                }
                else
                {
                    tcs.SetException(new InvalidOperationException("Failed to enqueue function."));
                }
            }
            catch (Exception e)
            {
                tcs.SetException(e);
            }
        }))
        {
            tcs.SetException(new InvalidOperationException("Failed to enqueue function."));
        }

        return tcs.Task;
    }
}