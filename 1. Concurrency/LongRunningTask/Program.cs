int count = 0;

object locked = new object();

await Task.WhenAll(Enumerable.Range(0,100).Select(ti => {
    return Task.Factory.StartNew(i => {
        Thread.Sleep(1000);
        lock(locked)
        {
            count++;
        }
        Console.WriteLine($"Task {i} complete");
    }, ti, TaskCreationOptions.LongRunning); // Попробуйте закомментировать TaskCreationOptions.LongRunning
}));

Console.WriteLine("Done!");
