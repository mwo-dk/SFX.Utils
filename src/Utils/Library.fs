namespace SFX.Utils

open SFX.ROP
open SFX.Utils.Infrastructure

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

    let initialize =
        let doInitialize x =
            match x with
            | Sync x -> x.Initialize()
            | Async x -> x.InitializeAsync() |> Async.AwaitTask |> Async.RunSynchronously
        let handleError error = error |> InitializationFailed
        tryCatch doInitialize handleError

    let isInitialized =
        let doIsInitilized x =
            match x with
            | Sync x -> x.IsInitialized()
            | Async x -> x.IsInitialized()
        let handleError error = error |> InitializationCheckFailed
        tryCatch doIsInitilized handleError

module Initializer =
    type InitializerObject =
    | Sync of IInitializer
    | Async of IAsyncInitializer
    
    let initialize =
        let doInitialize x =
            match x with
            | Sync x -> x.Initialize()
            | Async x -> x.InitializeAsync() |> Async.AwaitTask |> Async.RunSynchronously
        tryCatch doInitialize id        

module HashHelpers =
    type HashError = 
    | IsNull
    | IsEmpty

    let inline private hash' prime1 prime2 x = 
        x |> Array.fold (fun acc n -> acc*prime2 + n) prime1
    let inline private hash'' prime1 prime2 x =
        match x |> isNull with
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
            match x |> isNull with
            | false -> x |> Array.map (fun x -> x.GetHashCode()) |> hash'''
            | true -> IsNull |> fail

module StringHelpers =
    open System

    let isNullOrEmpty = String.IsNullOrEmpty
    let isNullOrWhiteSpace = String.IsNullOrWhiteSpace