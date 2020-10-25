using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

public class App
{
    public static void Main(string[] args)
    {
        const int MatrixSize = 800;
        
        double[,] a = new double[MatrixSize,MatrixSize];
        double[,] b = new double[MatrixSize,MatrixSize];
        Random r = new Random();
        for(int i =0;i<MatrixSize;i++)
            for (int j = 0; j < MatrixSize; j++)
            {
                a[i, j] = r.NextDouble();
                b[i, j] = r.NextDouble();
            }

        Console.WriteLine("Processors: {0}", Environment.ProcessorCount);

        Stopwatch sw = new Stopwatch();
        sw.Start();
        double[,] c = Multiply(a, b);
        sw.Stop();

        Console.WriteLine("Time elapsed: {0} ms", sw.ElapsedMilliseconds);

        Stopwatch sw2 = new Stopwatch();
        sw2.Start();
        double[,] c2 = MultiplyT(a, b);
        sw2.Stop();

        Console.WriteLine("Time elapsed (thread pool): {0} ms", sw2.ElapsedMilliseconds);



        Stopwatch sw3 = new Stopwatch();
        sw3.Start();
        double[,] c3 = MultiplyTa(a, b);
        sw3.Stop();

        Console.WriteLine("Time elapsed (TPL) {0} ms", sw3.ElapsedMilliseconds);
    }

    public static double[,] Multiply(double[,] a, double[,] b)
    {
        int n = a.GetLength(0);
        int m = b.GetLength(1);
        int l = a.GetLength(1);
        if (l != b.GetLength(0))
            throw new ArgumentException("Wrong shape of matrices");
        double[,] c = new double[a.GetLength(0), b.GetLength(1)];

        //Parallel.For(0, n, i =>
        for(int i =0;i<n;i++)
        {
            for (int j = 0; j < m; j++)
            {
                double sum = 0;
                for (int k = 0; k < l; k++)
                    sum += a[i, k] * b[k, j];
                c[i, j] = sum;

            }
        }
       // );
               
        return c;
    }

    public static double[,] MultiplyT(double[,] a, double[,] b)
    {
        int n = a.GetLength(0);
        int m = b.GetLength(1);
        int l = a.GetLength(1);
        if (l != b.GetLength(0))
            throw new ArgumentException("Wrong shape of matrices");
        double[,] c = new double[a.GetLength(0), b.GetLength(1)];

        var threads = new Thread[n];

        for(int i =0;i<n;i++)
        {
            threads[i] = new Thread(pi => {
                int idx = (int)pi;
                // idx and i will be different in some dumps
                // Console.WriteLine($"i = {i}, idx={idx}");
                for (int j = 0; j < m; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < l; k++)
                        sum += a[idx, k] * b[k, j];
                    c[idx, j] = sum;

                }
            });
            threads[i].Start(i);
        }

        for (int i = 0; i < n; i++)
            threads[i].Join();

        return c;
    }

    public static double[,] MultiplyTP(double[,] a, double[,] b)
    {
        int n = a.GetLength(0);
        int m = b.GetLength(1);
        int l = a.GetLength(1);
        if (l != b.GetLength(0))
            throw new ArgumentException("Wrong shape of matrices");
        double[,] c = new double[a.GetLength(0), b.GetLength(1)];

        var events = new AutoResetEvent[n];

        for (int i = 0; i < n; i++)
        {
            events[i] = new AutoResetEvent(false);
            ThreadPool.QueueUserWorkItem(pi =>
            {
                int idx = (int)pi;
                // idx and i will be different in some dumps
                // Console.WriteLine($"idx={idx};i={i}");
                for (int j = 0; j < m; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < l; k++)
                        sum += a[idx, k] * b[k, j];
                    c[idx, j] = sum;

                }
                events[idx].Set();
            }, i);
        }

        for (int i = 0; i < n; i++)
            events[i].WaitOne();

        // Next call fails for large matrices
        // WaitHandle.WaitAll(events);

        return c;
    }

    public static double[,] MultiplyTa(double[,] a, double[,] b)
    {
        int n = a.GetLength(0);
        int m = b.GetLength(1);
        int l = a.GetLength(1);
        if (l != b.GetLength(0))
            throw new ArgumentException("Wrong shape of matrices");
        double[,] c = new double[a.GetLength(0), b.GetLength(1)];

        var tasks = new Task[n];
        
        for (int i = 0; i < n; i++)
        {
            tasks[i] = Task.Factory.StartNew(pi =>
            {
                int idx = (int)pi;
                for (int j = 0; j < m; j++)
                {
                    double sum = 0;
                    for (int k = 0; k < l; k++)
                        sum += a[idx, k] * b[k, j];
                    c[idx, j] = sum;

                }
            }, i);
        }

        Task.WaitAll(tasks);
       
        return c;
    }

}
