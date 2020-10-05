using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;

namespace AzureComputerVision
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var key = Environment.GetEnvironmentVariable("COMPUTER_VISION_API_KEY");
            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(key))
                { Endpoint = "https://visiondemosb.cognitiveservices.azure.com/" };
            
            using var imageStream = File.OpenRead(args.FirstOrDefault() ?? "sample.jpg");

            using var cts = new CancellationTokenSource();
            _ = Task.Factory.StartNew(async () => {
                while(!cts.Token.IsCancellationRequested) {
                    Console.Write(".");
                    await Task.Delay(500);
                }
            }, cts.Token);

            var result = await client.DetectObjectsInStreamAsync(imageStream);
            
            cts.Cancel();

            Console.WriteLine();
            foreach(var obj in result.Objects)
            {
                Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence}");
            }
        }
    }
}
