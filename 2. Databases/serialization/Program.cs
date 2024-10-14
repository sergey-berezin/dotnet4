using Newtonsoft.Json;

var x = new double[] { 2.87, 3.1415 };
var y = new double[] { -1.0, 1.0 };

var bytes = new byte[4 * sizeof(double)];
Buffer.BlockCopy(x, 0, bytes, 0, 2 * sizeof(double));
Buffer.BlockCopy(y, 0, bytes, 2 * sizeof(double), 2 * sizeof(double));

foreach(var b in bytes)
    Console.Write($"{b:X02}");
Console.WriteLine();

var l = new List {
    x = 3.1415,
    next = new List {
        x = 2.87
    }
};
string s = JsonConvert.SerializeObject(l);
Console.WriteLine(s);

l.next.next = l;
string s2 = JsonConvert.SerializeObject(l, new JsonSerializerSettings {
    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
    PreserveReferencesHandling = PreserveReferencesHandling.Objects
});


class List
{
    public double x;
    public List next;
}