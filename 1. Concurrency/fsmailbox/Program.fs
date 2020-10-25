// Learn more about F# at http://fsharp.org

open System
open Microsoft.FSharp.Control

[<EntryPoint>]
let main argv =
    let processor = MailboxProcessor.Start(fun inbox ->
        let rec loop count = async {
            let! msg = inbox.Receive()
            printfn "%d: %s" count msg
            return! loop (count + 1)
        }
        loop 0)
 
    async {
        for i in 0..100 do
            processor.Post "A"
    } |> Async.Start

    async {
        for i in 0..100 do
            processor.Post "B"
    } |> Async.Start 
    
    System.Console.ReadLine() |> ignore
    0