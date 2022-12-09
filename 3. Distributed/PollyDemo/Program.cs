using Polly;

var jitterer = new Random(); 

var retryPolicy = Policy
    .Handle<HttpRequestException>()
    .WaitAndRetryAsync(5, 
        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))  // exponential back-off: 2, 4, 8 etc
                      + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)));  // plus some jitter: up to 1 second

using(var client = new HttpClient()) 
{
    var result = await retryPolicy.ExecuteAsync(async () => {
        Console.WriteLine("Getting data...");
        return await client.GetStringAsync("http://localhost/foobar");
    });
    Console.WriteLine(result);
}
                      
