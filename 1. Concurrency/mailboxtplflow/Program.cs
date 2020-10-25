using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace mailboxtplflow
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var ab = new ActionBlock<int>(async x => {
                var r = new Random();
                Console.Write("[");
                await Task.Delay(r.Next(1000));
                Console.WriteLine("]");
            });

            Parallel.For(0,100, i => ab.Post(i));
            ab.Complete();

            await ab.Completion;
        }
    }
}
