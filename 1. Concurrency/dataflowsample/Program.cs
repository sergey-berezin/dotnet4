using System.Threading.Tasks.Dataflow;

var input = new BufferBlock<int>();

var t1 = new TransformBlock<int, string>(async i => {
    var r = new Random();
    Console.WriteLine($"--- Start processing {i}...");
    await Task.Delay(r.Next(1000));
    Console.WriteLine($"--- Finish processing {i}...");
    return i.ToString();
}, new ExecutionDataflowBlockOptions() {
    MaxDegreeOfParallelism = 4,
});

var t2 = new ActionBlock<string>(async s => {
    var r = new Random();
    Console.WriteLine($"### Start processing {s}...");
    await Task.Delay(r.Next(1000));
    Console.WriteLine($"### Finish processing {s}...");             
});

input.LinkTo(t1, new DataflowLinkOptions() {
    PropagateCompletion = true
});

t1.LinkTo(t2, new DataflowLinkOptions() {
    PropagateCompletion = true
});

Parallel.For(0,10, i => {
    input.Post(i);
});
input.Complete();

await t2.Completion;