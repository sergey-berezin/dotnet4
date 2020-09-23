using System;
using System.Collections.Generic;
using System.Threading;

namespace readwrite
{
    public class App
    {

        protected static Queue<string> queue = new Queue<string>();

        public static void Main(string[] args)
        {
            Thread first = new Thread(new ThreadStart(Producer));
            first.Start();
            
            Thread second = new Thread(new ThreadStart(Consumer))
            {
                IsBackground = true
            };

            second.Start();
            first.Join(); 
        }

        public static void Producer() // Producer: writes to queue
        {
            while (true)
            {
                string s = Console.In.ReadLine();
                if (s == "")
                    break;
                lock(queue) // Занимает целое ядро 
                    queue.Enqueue(s);
            }
        }

        public static void Consumer() // Consumer: reads from queue
        {
            while (true)
            {
                lock (queue)
                {
                    while (queue.Count > 0)
                        Console.Out.WriteLine(queue.Dequeue());
                }
            }
        }
    }
}
