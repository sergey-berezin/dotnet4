using System;
using System.Collections.Generic;
using System.Threading;

namespace mailbox
{
    class Program
    {
        static SemaphoreSlim hasMessages = new SemaphoreSlim(0,1);
        static SemaphoreSlim boxLock = new SemaphoreSlim(1,1);
        static Queue<string> mailbox = new Queue<string>();

        static void Enqueue(string m) {
            boxLock.Wait();
            if(mailbox.Count == 0)
                hasMessages.Release();
            mailbox.Enqueue(m);
            boxLock.Release();
        }

        static void Process() {
            while(true) {
                hasMessages.Wait();
                boxLock.Wait();
                Queue<string> m = new Queue<string>();
                while(mailbox.Count > 0)
                    m.Enqueue(mailbox.Dequeue());
                boxLock.Release();
                while(m.Count > 0)
                    Console.WriteLine(m.Dequeue());
            }
        }

        static void Main(string[] args)
        {
            new Thread(Process).Start();

            new Thread(() => {
                for(var i = 0;i<100;i++) {
                    Enqueue($"T0: {i}");
                    Thread.Sleep(10);
                }
            }).Start();
            
            new Thread(() => {
                for(var i = 0;i<100;i++) {
                    Enqueue($"T1: {i}");
                    Thread.Sleep(10);
                }
            }).Start();
        }
    }
}
