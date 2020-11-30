using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using LibraryContracts;

namespace LibraryClient
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync("http://localhost:5000/api/Books");
            var allbooks = JsonConvert.DeserializeObject<Book[]>(result);
            foreach(var b in allbooks)
                Console.WriteLine($"{b.Title}");

            var nb = new NewBook() { Title = "C", Pages = 400 };
            var s = JsonConvert.SerializeObject(nb);
            var c = new StringContent(s);
            c.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            await client.PutAsync("http://localhost:5000/api/books", c);


            return 0;
        }
    }
}
