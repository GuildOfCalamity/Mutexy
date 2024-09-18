using System.Threading;

namespace Mutexy;

internal class Program
{
    static int maxThreads = 6;
    static ValueStopwatch vsw = ValueStopwatch.StartNew();

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        if (args.Length > 0)
        {
            if (Int32.TryParse(args[0], out int tmp))
                maxThreads = tmp;
        }
        ThreadSafeResource resource = new ThreadSafeResource();
        Thread[] _workers = new Thread[maxThreads];
        for (int i = 0; i < maxThreads; i++)
        {
            int tnum = i;
            (_workers[i] = new Thread(() =>
            {
                while (!resource.AccessResource($"Thread #{tnum}"))
                {
                    var sleep = Random.Shared.Next(2000,5000);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"⇨ Thread #{tnum} failed to access the resource, trying again in {new TimeSpan(0, 0, 0, 0, sleep).ToReadableString()}...");
                    Console.WriteLine($"⇨ Thread resource is currently \"{resource.Property}\"");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Thread.Sleep(sleep);
                }
            })
            { IsBackground = true }).Start();
        }
        for (int i = 0; i < maxThreads; i++) { _workers[i].Join(); }
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"⇨ Test took {vsw.GetElapsedTime().TotalSeconds:N1} seconds");
        Console.WriteLine($"⇨ Test completed. Press any key to exit.");
        Console.ForegroundColor = ConsoleColor.Gray;
        _ = Console.ReadKey(true);
    }
}
