namespace Mutexy;

internal class Program
{
    static int maxThreads = 10;
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        ThreadSafeResource resource = new ThreadSafeResource();
        Thread[] _workers = new Thread[maxThreads];
        for (int i = 0; i < maxThreads; i++)
        {
            int tnum = i;
            (_workers[i] = new Thread(() =>
            {
                while (!resource.AccessResource($"Thread #{tnum}"))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"⇨ Thread #{tnum} failed to access the resource, trying again...");
                    Console.WriteLine($"⇨ Thread resource is currently \"{resource.Property}\"");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Thread.Sleep(2000);
                }
            })
            { IsBackground = true }).Start();
        }
        for (int i = 0; i < maxThreads; i++) { _workers[i].Join(); }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"⇨ Test completed. Press any key to exit.");
        Console.ForegroundColor = ConsoleColor.Gray;
        _ = Console.ReadKey(true);
    }
}
