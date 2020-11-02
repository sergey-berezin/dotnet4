using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace json
{
    public struct Complex 
    { 
        public double Re { get; set; }

        public double Im { get; set;}
    }



    class Program
    {
        static void Main(string[] argst)
        {
            // Чтение и работа с JSON
            var json = JToken.Parse(File.ReadAllText("data.json"));
            dynamic d = json;
            var x = json["Manufacturers"][0]["Products"][0]["Price"];
            Console.WriteLine(x.ToString());
            var x2 = d.Manufacturers[0].Products[0].Price;
            Console.WriteLine(x2.ToString());

            // JSONPath
            var p = json.SelectTokens("$..Products[?(@.Price >= 90)].Name").FirstOrDefault();
            Console.WriteLine(p.ToString());

            // Сериализация в JSON
            var z = new Complex[] {
                new Complex() { Re = 0, Im = 1 }
            };

            string s = JsonConvert.SerializeObject(z); 
            Console.WriteLine(s);  
            var z2 = JsonConvert.DeserializeObject<Complex[]>(s);
            Console.WriteLine($"{z2[0].Im}"); 
        }
    }
}
