namespace Serilog.Sinks.AmazonSQS;

public static class TaskSafeExtension
{
    public static T SyncContextSafeWait<T>(this Task<T> task, int timeout = -1)
    {
        var current = SynchronizationContext.Current;
        try
        {
            return task.Wait(timeout) ? task.Result : throw new TimeoutException("Send message timeout.");
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext(current);
        }
    }
}