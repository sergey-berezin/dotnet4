using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

public class App
{
    public static async Task Main(string[] args)
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

        CancellationTokenSource ctf = new CancellationTokenSource();

        Stopwatch sw = new Stopwatch();
        sw.Start();
        var t1 = MultiplyAsync(a, b, ctf.Token);
        var t2 = MultiplyAsync(b, a, ctf.Token);
        // ctf.Cancel();
        var results = await Task.WhenAll(new Task<double[,]>[] { t1, t2 });
        var dist = await DistanceAsync(t1.Result, t2.Result, ctf.Token);    
        
        sw.Stop();

        Console.WriteLine("Time elapsed {0} ms", sw.ElapsedMilliseconds);
        Console.WriteLine($"Distance: {dist}");
    }

    public static async Task<double> DistanceAsync(double[,] a, double[,] b, CancellationToken token)
    {
        int n = a.GetLength(0);
        int m = a.GetLength(1);
        if (n != b.GetLength(0))
            throw new ArgumentException("First dimension does not match");
        if(m != b.GetLength(1))
            throw new ArgumentException("Second dimension does not match");
        
        Task<double>[] tasks = new Task<double>[n];
        for (int i = 0; i < n; i++)
        {
            tasks[i] = Task.Factory.StartNew<double>(pi => {
                int ii = (int)pi;
                double sum = 0;
                for(int j = 0;j<m;j++)
                    sum += (a[ii,j] - b[ii,j]) * (a[ii,j] - b[ii,j]);
                return sum;
            }, i, token);
        }

        var sums = await Task.WhenAll(tasks);
        return Math.Sqrt(sums.Sum());
    }

    public static async Task<double[,]> MultiplyAsync(double[,] a, double[,] b, CancellationToken token)
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
                    token.ThrowIfCancellationRequested();
                    double sum = 0;
                    for (int k = 0; k < l; k++)
                        sum += a[idx, k] * b[k, j];
                    c[idx, j] = sum;

                }
            }, i, token);
        }

        await Task.WhenAll(tasks);
        return c;
    }
}
