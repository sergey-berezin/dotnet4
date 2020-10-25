using System;
using System.Threading.Tasks;
using System.Linq;

namespace Continuations
{
    class Program
    {
        static async Task<int> PrintAsync(char ch, int count) {
            return await Task<int>.Factory.StartNew(() => {
                Console.Write("Task 1 started!");
                for(int i = 0;i<count;i++)
                    Console.Write(ch);
                return count;
            });
        }

        static async Task<int> M1() {
            int a = await PrintAsync('.', 80);
            int b = await PrintAsync('*', 80);
            int c = await PrintAsync('$', 80);
            return a + b + c;
        }           

        static async Task<int> M2() {
            int a = await PrintAsync('#', 80);
            int b = await PrintAsync('&', 80);
            int c = await PrintAsync('@', 80);
            return a + b + c;
        }   

        static async Task Main(string[] args)
        {
            await Task.WhenAll<int>(M1(), M2());
        }
    }
}
