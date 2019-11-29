namespace SFX.Utils

open SFX.ROP
open SFX.Utils.Infrastructure

module StringHelpers =
    open System

    let isNullOrEmpty = String.IsNullOrEmpty
    let isNullOrWhiteSpace = String.IsNullOrWhiteSpace

module HashHelpers =
    type HashErrors = 
    | IsNull
    | IsEmpty

    let inline private hash' prime1 prime2 x = 
        x |> Array.fold (fun acc n -> acc*prime2 + n) prime1
    let inline private hash'' prime1 prime2 x =
        match x = null with
        | false ->
            match x |> Array.isEmpty with
            | false -> x |> hash' prime1 prime2 |> succeed
            | true -> IsEmpty |> fail
        | true -> IsNull |> fail
    let inline private hash''' x = x |> hash'' 19 31
    let inline private hash'''' (convert: 'a -> int) x =
        x |> Array.map convert |> hash'''

    type HashArgs =
    | B of byte array
    | I of int array
    | O of obj array
    let byteArray = B
    let intArray = I
    let objArray = O

    let hash x =
        match x with
        | B x -> x |> hash'''' (fun b -> int(b))
        | I x -> x |> hash'''
        | O x -> 
            match x = null with
            | false -> x |> Array.map (fun x -> x.GetHashCode()) |> hash'''
            | true -> IsNull |> fail

module DateTime =
    open System

    let getUtcNow() = DateTimeOffset.UtcNow
  
    let createDateTimeProvider() = DateTimeProvider()

module Timer =
    open Microsoft.FSharp.Core
    open System

    let private timerProvider = TimerProvider()
    let createTimerProvider() = TimerProvider()

    let createTimer interval (handler: unit -> unit) autoStart =
        timerProvider.Create(interval, System.Action(handler), autoStart)

    type TimerError =
    | TimerDisposed
    | Other of exn

    let startTimer (timer: ITimer) =
        try
            timer.Start() |> succeed
        with
        | :? ObjectDisposedException -> TimerDisposed |> fail
        | exn -> exn |> Other |> fail

    let stopTimer (timer: ITimer) =
        try
            timer.Stop() |> succeed
        with
        | :? ObjectDisposedException -> TimerDisposed |> fail
        | exn -> exn |> Other |> fail

    let closeTimer (timer: ITimer) = timer.Dispose()

module Initializable =
    type InitializableObject =
    | Sync of IInitializable
    | Async of IAsyncInitializable
    let sync = Sync
    let async = Async

    type InitializationError =
    | InitializationFailed of exn
    | InitializationCheckFailed of exn

    let initialize x =
        match x with
        | Sync x -> tryCatch (fun _ -> x.Initialize()) (fun error -> error |> InitializationFailed)
        | Async x -> tryCatch (fun _ -> x.InitializeAsync() |> Async.AwaitTask |> Async.RunSynchronously) (fun error -> error |> InitializationFailed)

    let isInitialized x =
        match x with
        | Sync x -> tryCatch (fun _ -> x.IsInitialized()) InitializationCheckFailed
        | Async x -> tryCatch (fun _ -> x.IsInitialized()) InitializationCheckFailed

module Initializer =

    type InitializerObject =
    | Sync of IInitializer
    | Async of IAsyncInitializer

    type InitializationError =
    | InitializationFailed of exn
    | InitializationCheckFailed of exn
    
    let initialize x =
        match x with
        | Sync x -> tryCatch (fun _ -> x.Initialize()) id
        | Async x -> tryCatch (fun _ -> x.InitializeAsync() |> Async.AwaitTask |> Async.RunSynchronously) id
