using System;
using System.Collections.Concurrent;
using System.Threading;

namespace jobqueue
{
    class Program
    {
        static void Main(string[] args)
        {
            ConcurrentQueue<int> jobs = new ConcurrentQueue<int>();

            int numProcs = Environment.ProcessorCount;
            
            for(int i = 0;i<numProcs;i++)
                new Thread(() => {
                    while(true) {
                        if(jobs.TryDequeue(out int job)) {
                            Console.WriteLine($"{job}");
                        } else
                            Thread.Sleep(0);
                    }
                }).Start();

            for(int i = 0;i<100;i++)
                jobs.Enqueue(i);
        }
    }
}
