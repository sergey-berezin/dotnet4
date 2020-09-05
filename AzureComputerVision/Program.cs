using System;
using System.IO;
using System.Linq;
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
            var result = await client.DetectObjectsInStreamAsync(imageStream);
          
            foreach(var obj in result.Objects)
            {
                Console.WriteLine($"{obj.ObjectProperty} with confidence {obj.Confidence}");
            }
        }
    }
}
