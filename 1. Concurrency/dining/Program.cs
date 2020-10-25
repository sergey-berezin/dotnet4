using System;
using System.Linq;
using System.Threading;

namespace dining
{
    class Program
    {
        static void Main(string[] args)
        {
            var forks = Enumerable.Range(0,5).Select(_ => new SemaphoreSlim(1,1)).ToArray();
            var threads = Enumerable.Range(0,5).Select(i => {
                var t = new Thread(o =>
                {
                    while(true) 
                    {
                        int no = (int)o;
                        if(no == 4) {
                            forks[0].Wait();
                            forks[4].Wait();                     
                        } else {
                            forks[no].Wait();
                            forks[no + 1].Wait();
                        }
                        ///forks[no].Wait();
                        //forks[(no + 1) % 5].Wait();
                        Console.WriteLine($"{no+1} is eating...");
                        //Thread.Sleep(10);
                        forks[(no+1) % 5].Release();
                        forks[no].Release();
                        Console.WriteLine($"{no+1} is sleeping...");
                        //Thread.Sleep(10);
                    }
                });
                t.Start(i);
                return t;
            }).ToArray();
            
            foreach(var t in threads)
                t.Join();
        }
    }
}
