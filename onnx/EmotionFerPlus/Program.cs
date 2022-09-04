// Download ONNX model from https://github.com/onnx/models/blob/main/vision/body_analysis/emotion_ferplus/model/emotion-ferplus-7.onnx
// to project directory before build

using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using var session = new InferenceSession("emotion-ferplus-7.onnx");  

foreach(var kv in session.InputMetadata)
    Console.WriteLine($"{kv.Key}: {MetadataToString(kv.Value)}");
foreach(var kv in session.OutputMetadata)
    Console.WriteLine($"{kv.Key}: {MetadataToString(kv.Value)}]");

using Image<Rgb24> image = Image.Load<Rgb24>("face1.png");
image.Mutate(ctx => {
    ctx.Resize(new Size(64,64));
   // ctx.Grayscale();
});

var inputs = new List<NamedOnnxValue> { NamedOnnxValue.CreateFromTensor("Input3", GrayscaleImageToTensor(image)) };
using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = session.Run(inputs);
var emotions = Softmax(results.First(v => v.Name == "Plus692_Output_0").AsEnumerable<float>().ToArray());

string[] keys = { "neutral", "happiness", "surprise", "sadness", "anger", "disgust", "fear", "contempt" };
foreach(var i in keys.Zip(emotions))
    Console.WriteLine($"{i.First}: {i.Second}");


DenseTensor<float> GrayscaleImageToTensor(Image<Rgb24> img)
{
    var w = img.Width;
    var h = img.Height;
    var t = new DenseTensor<float>(new[] { 1, 1, h, w });

    img.ProcessPixelRows(pa => 
    {
        for (int y = 0; y < h; y++)
        {           
            Span<Rgb24> pixelSpan = pa.GetRowSpan(y);
            for (int x = 0; x < w; x++)
            {
                t[0, 0, y, x] = pixelSpan[x].R; // B and G are the same
            }
        }
    });
    
    return t;
}

string MetadataToString(NodeMetadata metadata)
    => $"{metadata.ElementType}[{String.Join(",", metadata.Dimensions.Select(i => i.ToString()))}]";

float[] Softmax(float[] z)
 {
    var exps = z.Select(x => Math.Exp(x)).ToArray();
    var sum = exps.Sum();
    return exps.Select(x => (float)(x / sum)).ToArray();
}