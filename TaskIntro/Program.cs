using System;
using System.Threading;
using System.Threading.Tasks;

namespace threadintro
{
    class Program
    {
        static void Main(string[] args)
        {
            int count = 0;

            var t = Task.Factory.StartNew(() => {
                for(int i = 0;i<100000000;i++) 
                {
                    Interlocked.Increment(ref count);
                    if(i%10000000==0) 
                        Console.Write("*");
                }
            });

            for(int i = 0;i<100000000;i++) 
            {
                Interlocked.Decrement(ref count);
                if(i%10000000==0)
                    Console.Write(".");
            }

            t.Wait();
            Console.WriteLine($"\n{count}");
        }
    }
}