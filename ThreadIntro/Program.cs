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
            
            var th = new Thread(new ThreadStart(() =>
            {
                for(int i = 0;i<100000000;i++) 
                {
                    Interlocked.Increment(ref count);
                    if(i%10000000==0)
                        Console.Write("*");
                }
            }));
            th.Start();

            for(int i = 0;i<100000000;i++) 
            {
                Interlocked.Decrement(ref count);
                if(i%10000000==0)
                    Console.Write(".");
            }

            th.Join();
            Console.WriteLine($"\n{count}");
        }
    }
}