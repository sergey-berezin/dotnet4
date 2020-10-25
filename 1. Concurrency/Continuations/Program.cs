using System;
using System.Threading.Tasks;
using System.Linq;

namespace Continuations
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = Task<int>.Factory.StartNew(() => {
                for(int i = 0;i<1000;i++)
                    Console.Write(".");
                return 1000;
            }).ContinueWith(prevTask => {
                for(int i = 0;i<prevTask.Result;i++)
                    Console.Write("$");
                return prevTask.Result * 2;
            });

            var task2 = Task<int>.Factory.StartNew(() => {
                for(int i = 0;i<3000;i++)
                    Console.Write("*");
                return 20000;
            });

            var task3 = Task.WhenAll<int>(task, task2).ContinueWith(combined => {
                for(int i = 0;i<1000;i++)
                    Console.Write("?");
                return combined.Result.Sum();
            });

            Console.WriteLine($"{task3.Result}");
        }
    }
}
