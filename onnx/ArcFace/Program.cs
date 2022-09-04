
// Download ONNX model from https://github.com/onnx/models/blob/main/vision/body_analysis/arcface/model/arcfaceresnet100-8.onnx
// to project directory before run

using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using var session = new InferenceSession("arcfaceresnet100-8.onnx");  
Console.WriteLine("Predicting contents of image..."); 

foreach(var kv in session.InputMetadata)
    Console.WriteLine($"{kv.Key}: {MetadataToString(kv.Value)}");
foreach(var kv in session.OutputMetadata)
    Console.WriteLine($"{kv.Key}: {MetadataToString(kv.Value)}]");

using var face1 = Image.Load<Rgb24>("face1.png");
using var face2 = Image.Load<Rgb24>("face2.png");

var embeddings1 = GetEmbeddings(face1);
var embeddings2 = GetEmbeddings(face2);

Console.WriteLine($"Distance =  {Distance(embeddings1, embeddings2) * Distance(embeddings1, embeddings2)}");
Console.WriteLine($"Similarity =  {Similarity(embeddings1, embeddings2)}");

string MetadataToString(NodeMetadata metadata)
    => $"{metadata.ElementType}[{String.Join(",", metadata.Dimensions.Select(i => i.ToString()))}]";

float Length(float[] v) => (float)Math.Sqrt(v.Select(x => x*x).Sum());

float[] Normalize(float[] v) 
{
    var len = Length(v);
    return v.Select(x => x / len).ToArray();
}

float Distance(float[] v1, float[] v2) => Length(v1.Zip(v2).Select(p => p.First - p.Second).ToArray());

float Similarity(float[] v1, float[] v2) => v1.Zip(v2).Select(p => p.First * p.Second).Sum();

DenseTensor<float> ImageToTensor(Image<Rgb24> img)
{
    var w = img.Width;
    var h = img.Height;
    var t = new DenseTensor<float>(new[] { 1, 3, h, w });

    img.ProcessPixelRows(pa => 
    {
        for (int y = 0; y < h; y++)
        {           
            Span<Rgb24> pixelSpan = pa.GetRowSpan(y);
            for (int x = 0; x < w; x++)
            {
                t[0, 0, y, x] = pixelSpan[x].R;
                t[0, 1, y, x] = pixelSpan[x].G;
                t[0, 2, y, x] = pixelSpan[x].B;
            }
        }
    });
    
    return t;
}

float[] GetEmbeddings(Image<Rgb24> face) 
{
    var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor("data", ImageToTensor(face)) };
    using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = session.Run(inputs);
    return Normalize(results.First(v => v.Name == "fc1").AsEnumerable<float>().ToArray());
}