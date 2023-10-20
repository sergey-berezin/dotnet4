using Microsoft.Research.Science.Data;
using System.Linq;
using System.Runtime.Versioning;

var x = Enumerable.Range(0,201).Select(i => -Math.PI + i * Math.PI /100).ToArray();
var y = Enumerable.Range(0,101).Select(i => i * Math.PI /100).ToArray();

var f = new double[x.Length, y.Length];
for(int i = 0;i<x.Length;i++)
    for(int j = 0;j<y.Length;j++)
        f[i,j] = Math.Cos(x[i]) * Math.Sin(y[j]);
 
using var dataSet = DataSet.Open("grid.nc");
dataSet.AddVariable<double>("x", x, "i");
dataSet.AddVariable<double>("y", y, "j");
dataSet.AddVariable<double>("f", f, "i", "j");