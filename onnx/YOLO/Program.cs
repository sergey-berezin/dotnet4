using System;
using SixLabors.ImageSharp; // Из одноимённого пакета NuGet
using SixLabors.ImageSharp.PixelFormats;
using System.Linq;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;
using System.Collections.Generic;

namespace YOLO_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            using var image = Image.Load<Rgb24>(args.FirstOrDefault() ?? "chair.jpg");
            
            int imageWidth = image.Width;
            int imageHeight = image.Height;

            // Размер изображения
            const int TargetWidth = 416;
            const int TargetHeight = 416;

            

            // Изменяем размер изображения до 416 x 416
            var resized = image.Clone(x =>
            {
                x.Resize(new ResizeOptions
                {
                    Size = new Size(TargetWidth, TargetHeight),
                    Mode = ResizeMode.Pad // Дополнить изображение до указанного размера
                });
            });

             // Перевод пикселов в тензор и нормализация
            var input = new DenseTensor<float>(new[] { 1, 3, TargetHeight, TargetWidth });
            for (int y = 0; y < TargetHeight; y++)
            {           
                Span<Rgb24> pixelSpan = resized.GetPixelRowSpan(y);
                for (int x = 0; x < TargetWidth; x++)
                {
                    input[0, 0, y, x] = pixelSpan[x].R;
                    input[0, 1, y, x] = pixelSpan[x].G;
                    input[0, 2, y, x] = pixelSpan[x].B;
                }
            }

            // Подготавливаем входные данные нейросети. Имя input задано в файле модели
            var inputs = new List<NamedOnnxValue>  
            { 
               NamedOnnxValue.CreateFromTensor("image", input),
            };

            // Вычисляем предсказание нейросетью
            using var session = new InferenceSession("tinyyolov2-7.onnx");  
            using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = session.Run(inputs);
            
            // Получаем результаты
            var outputs = results.First().AsTensor<float>();

            const int CellCount = 13; // 13x13 ячеек
            const int BoxCount = 5; // 5 прямоугольников в каждой ячейке
            const int ClassCount = 20; // 20 классов

            string[] labels = new string[]
            {
                "aeroplane", "bicycle", "bird", "boat", "bottle",
                "bus", "car", "cat", "chair", "cow",
                "diningtable", "dog", "horse", "motorbike", "person",
                "pottedplant", "sheep", "sofa", "train", "tvmonitor"
            };

            float Sigmoid(float value)
    	    {
                var e = (float)Math.Exp(value);
                return e / (1.0f + e);
            } 

            var bestConf = Double.MinValue;
            int bestRow = -1, bestCol = -1, bestBbox = -1; 
            List<float> confs = new List<float>();
            for(var row = 0;row < CellCount;row++)
                for(var col = 0;col < CellCount;col++)
                    for(var bbox = 0;bbox<BoxCount;bbox++)
                    {
                        var conf = Sigmoid(outputs[0,(5 + ClassCount)*bbox + 4,row,col]);
                        confs.Add(conf);
                        Console.WriteLine(conf);
                        if(conf > bestConf) 
                        {
                            bestConf = conf;
                            bestRow = row;
                            bestCol = col;
                            bestBbox = bbox;
                        }
                    }

            var classes = 
                Enumerable.Range(0,ClassCount)
                .Select(i => outputs[0,(5 + ClassCount)*bestBbox + 5 + i,bestRow,bestCol])
                .ToArray();

            int bestClass = 0;
            for(var cls = 1;cls < ClassCount;cls++)
                if(classes[bestClass] < classes[cls])
                    bestClass = cls;

            Console.WriteLine($"Best confidence {bestConf} of best class {labels[bestClass]} at cell ({bestRow},{bestCol})");
            Console.WriteLine(
                String.Join(',',
                    confs
                    .OrderByDescending(c => c)
                    .Take(10)
                    .Select(c => c.ToString())));

            var outX = outputs[0, (5 + ClassCount)*bestBbox, bestRow, bestCol];
            var outY = outputs[0, (5 + ClassCount)*bestBbox + 1, bestRow, bestCol];
            var outW = outputs[0, (5 + ClassCount)*bestBbox + 2, bestRow, bestCol];
            var outH = outputs[0, (5 + ClassCount)*bestBbox + 3, bestRow, bestCol]; 

            double[] anchors = new double[]
            {
                1.08, 1.19, 3.42, 4.41, 6.63, 11.38, 9.42, 5.11, 16.62, 10.52
            };

            const int CellWidth = 32; // 416 / 13
            const int CellHeight = 32; // 416 / 13

            var X = ((float)bestCol+ Sigmoid(outX)) * CellWidth;
            var Y = ((float)bestRow + Sigmoid(outY)) * CellHeight;
            var width = (float)(Math.Exp(outW) * CellWidth * anchors[bestBbox * 2]);
            var height = (float)(Math.Exp(outH) * CellHeight * anchors[bestBbox * 2 + 1]);
            X -= width/2;
            Y -= height/2;

            // for(var cls = 0;cls<80;cls++)
            //     for(var obj = 0;obj < scores.Dimensions[2];obj++)
            //         if(scores[0, cls, obj] > bestScore) 
            //         {
            //             bestScore = scores[0, cls, obj];
            //             bestClass = cls;
            //             bestObj = obj;
            //         }
            // Console.WriteLine($"Best score {bestScore} of class {bestClass}");

            // var xc = boxes[0, bestObj, 0];
            // var yc = boxes[0, bestObj, 1];
            // var width = boxes[0, bestObj, 2];
            // var height = boxes[0, bestObj, 3];

            resized.Mutate(
                ctx => ctx.DrawPolygon(
                    Pens.Dash(Color.Red, 2),
                    new PointF[] {
                        new PointF(X, Y),
                        new PointF(X + width, Y),
                        new PointF(X + width, Y + height),
                        new PointF(X, Y + height)
                    }));
            resized.Save("result.jpg");
        }
    }
}
