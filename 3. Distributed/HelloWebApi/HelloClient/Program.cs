using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace HelloClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            string result = await client.GetStringAsync("http://localhost:5000/hello?name=World");
            Console.WriteLine(result);
        }
    }
}
