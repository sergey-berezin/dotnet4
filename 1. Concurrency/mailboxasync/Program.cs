using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace mailbox
{
    class Program
    {
        static SemaphoreSlim hasMessages = new SemaphoreSlim(0,1);
        static SemaphoreSlim boxLock = new SemaphoreSlim(1,1);
        static Queue<string> mailbox = new Queue<string>();

        static async Task Enqueue(string m) {
            await boxLock.WaitAsync();
            if(mailbox.Count == 0)
                hasMessages.Release();
            mailbox.Enqueue(m);
            boxLock.Release();
        }

        static async Task Process() {
            while(true) {
                await hasMessages.WaitAsync();
                await boxLock.WaitAsync();
                Queue<string> m = new Queue<string>();
                while(mailbox.Count > 0)
                    m.Enqueue(mailbox.Dequeue());
                boxLock.Release();
                while(m.Count > 0)
                    Console.WriteLine(m.Dequeue());
            }
        }

        static async Task Main(string[] args)
        {
            _ = Task.Run(async () => {
                for(var i = 0;i<100;i++)
                    await Enqueue($"T0: {i}");
            });
            _ = Task.Run(async () => {
                for(var i = 0;i<100;i++)
                    await Enqueue($"T1: {i}");
            });
            await Task.Run(Process);
        }
    }
}
