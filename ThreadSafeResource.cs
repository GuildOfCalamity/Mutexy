namespace Mutexy;

public class ThreadSafeResource
{
    readonly int _timeout = 1000;
    readonly Mutex _mutex = new Mutex(false, @"Global\ImTooMutexy");
    volatile string? _property;
    public string? Property
    {
        get => _property;
        set => _property = value;
    }

    // Method that simulates a critical section of code.
    public bool AccessResource(string threadName)
    {
        Console.WriteLine($"⇒ {threadName} is requesting the resource (I'll only wait for {new TimeSpan(0, 0, 0, 0, _timeout).ToReadableString()})");
        bool lockAcquired = _mutex.WaitOne(_timeout);
        if (lockAcquired)
        {
            try
            {
                var sleep = Random.Shared.Next(4000);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"⇒ {threadName} has entered the critical section ({sleep}ms)");
                Console.ForegroundColor = ConsoleColor.Gray;

                // Simulate some work with the shared resource.
                _property = threadName;
                Thread.Sleep(sleep);

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine($"⇒ {threadName} is leaving the critical section ({sleep}ms)");
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