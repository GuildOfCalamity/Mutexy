namespace Mutexy;

public class ThreadSafeResource
{
    readonly int _waitTimeout = 1000;
    readonly Mutex _mutex = new Mutex(false, @"Global\ImTooMutexy");

    /// <summary>
    /// For performance/optimization reasons, field memory locations may be 
    /// rearranged for certain kinds of reads/writes. Fields that are declared 
    /// volatile are excluded from certain kinds of optimizations.
    /// On a multiprocessor system, a volatile read operation does not guarantee 
    /// to obtain the latest value written to that memory location by any processor. 
    /// Similarly, a volatile write operation does not guarantee that the value 
    /// written would be immediately visible to other processors.
    /// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/volatile
    /// </summary>
    volatile string? _property;
    public string? Property
    {
        get => _property;
        set => _property = value;
    }

    /// <summary>
    /// Method to simulate shared access of a resource.
    /// </summary>
    /// <param name="threadName">the name of the thread requesting access</param>
    /// <returns>true if access granted, false if access blocked</returns>
    public bool AccessResource(string threadName)
    {
        Console.WriteLine($"⇒ {threadName} is requesting the resource (I'll only wait for {new TimeSpan(0, 0, 0, 0, _waitTimeout).ToReadableString()})");
        bool lockAcquired = _mutex.WaitOne(_waitTimeout);
        if (lockAcquired)
        {
            try
            {
                var sleep = Random.Shared.Next(4000);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"⇒ {threadName} has entered the critical section and will remain for {new TimeSpan(0, 0, 0, 0, sleep).ToReadableString()}");
                Console.ForegroundColor = ConsoleColor.Gray;

                // Simulate some work with the shared resource.
                _property = threadName;
                Thread.Sleep(sleep);

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"⇒ {threadName} is leaving the critical section");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            finally
            {
                // Allow other threads to access the resource.
                _mutex.ReleaseMutex();
            }
            return true;
        }
        else
        {
            Console.WriteLine($"⇒ {threadName} could not acquire the lock within the timeout period.");
            return false;
        }
    }
}