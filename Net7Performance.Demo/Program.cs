using System.Diagnostics;

var rwl = new ReaderWriterLockSlim();
var tasks = new Task[100];
var count = 0;

var end = DateTime.UtcNow + TimeSpan.FromSeconds(10);
while (DateTime.UtcNow < end)
{
    for (var i = 0; i < 100; ++i)
    {
        tasks[i] = Task.Run(() =>
        {
            var sw = Stopwatch.StartNew();
            rwl.EnterReadLock();
            rwl.ExitReadLock();
            sw.Stop();
            if (sw.ElapsedMilliseconds >= 10)
            {
                Console.WriteLine(Interlocked.Increment(ref count));
            }
        });
    }

    Task.WaitAll(tasks);
}