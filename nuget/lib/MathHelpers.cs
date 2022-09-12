using Flee.PublicTypes;

namespace NuGetSample {
    public static class MathHelpers 
    {
        public static double ArgMax(string expression,double xmin, double xmax)
        {
            ExpressionContext ctx = new ();
            ctx.Imports.AddType(typeof(Math));
            ctx.Variables.Add("x", 0.0);

            var expr = ctx.CompileGeneric<double>(expression);
            var grid = Enumerable
                .Range(0, 101)
                .Select(i => xmin + (xmax - xmin) * i / 100.0)
                .Select(x => {
                    ctx.Variables["x"] = x;
                    return (X: x, Y: expr.Evaluate());
                })
                .ToArray();

            return grid.MaxBy(p => p.Y).X;
        }
    }
}
