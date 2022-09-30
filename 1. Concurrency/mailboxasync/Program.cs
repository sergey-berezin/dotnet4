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
        static Queue<(string Input, TaskCompletionSource<string> Result)> mailbox = new();

        static CancellationTokenSource cts = new CancellationTokenSource();

        static async Task<string> Enqueue(string m) {
            await boxLock.WaitAsync();
            if(mailbox.Count == 0)
                hasMessages.Release();
            var r = new TaskCompletionSource<string>();
            mailbox.Enqueue((m,r));
            boxLock.Release();
            return await r.Task;
        }

        static async Task Process() {
            while(!cts.Token.IsCancellationRequested) {
                await hasMessages.WaitAsync();
                await boxLock.WaitAsync();
                var m = new Queue<(string Input, TaskCompletionSource<string> Result)>();
                while(mailbox.Count > 0)
                    m.Enqueue(mailbox.Dequeue());
                boxLock.Release();
                while(m.Count > 0) 
                {
                    var item = m.Dequeue();

                    // Выполняем полезные вычисления
                    var result = item.Input.ToLower();

                    item.Result.SetResult(result);
                }
            }
        }

        static async Task Main(string[] args)
        {
            var task0 = Task.Run(async () => {
                for(var i = 0;i<100;i++)
                {
                    var s = await Enqueue($"T0: {i}");
                    Console.WriteLine(s);
                }
            });
            var task1 = Task.Run(async () => {
                for(var i = 0;i<100;i++)
                {
                    var s = await Enqueue($"T1: {i}");
                    Console.WriteLine(s);
                }
            });

            var processTask = Task.Run(Process);

            // Ждём окончания двух задач, генерирующих данные
            await Task.WhenAll(task0, task1);

            // Завершаем цикл обработки
            cts.Cancel();
            await processTask;
        }
    }
}
