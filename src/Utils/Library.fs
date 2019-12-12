namespace SFX.Utils

open SFX.ROP
open SFX.Utils.Infrastructure

[<AutoOpen>]
module Helpers =
    let internal toRop (x: Result<'a>) =
        if x.Error |> isNull then x.Error |> fail
        else x.Value |> succeed

module Time =

    let private dateTimeProvider = DateTimeProvider()
    let createDateTimeProvider = DateTimeProvider
    let private timeZoneProvider = TimeZoneProvider()
    let createTimeZoneProvider = TimeZoneProvider
    let private dateTimeConverter = DateTimeConverter(timeZoneProvider)
    let createDateTimeConverter = DateTimeConverter
    
    let getNow = dateTimeProvider.GetNow
    let getUtcNow = dateTimeProvider.GetUtcNow

    let getLocalTimeZone = timeZoneProvider.GetLocal
    let getUtcTimeZone = timeZoneProvider.GetUtc
    let findSystemTimeZoneById = 
        timeZoneProvider.FindSystemTimeZoneById >> toRop

    let convert dt tz = dateTimeConverter.Convert(dt, tz) |> toRop
    let toUtc = dateTimeConverter.ToUtc >> toRop

module Timer =
    open Microsoft.FSharp.Core
    open System

    let private timerProvider = TimerProvider()
    let createTimerProvider = TimerProvider

    let createTimer interval (handler: unit -> unit) autoStart =
        timerProvider.Create(interval, System.Action(handler), autoStart) |> toRop

    type TimerError =
    | Null
    | TimerDisposed
    | Other of exn

    let private toRop (x: Result<'a>) =
        if x.Error |> isNull then x.Error |> Other |> fail
        else x.Value |> succeed
    let startTimer (timer: ITimer) =
        if timer |> isNull then Null |> fail
        else timer.Start() |> toRop
    let stopTimer (timer: ITimer) =
        if timer |> isNull then Null |> fail
        else timer.Stop() |> toRop
    let closeTimer (timer: ITimer) = 
        if timer |> isNull then Null |> fail
        else timer.Dispose() |> succeed

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
    | Null
    | Empty

    let inline private hash' prime1 prime2 x = 
        x |> Array.fold (fun acc n -> acc*prime2 + n) prime1
    let inline private hash'' prime1 prime2 x =
        match x |> isNull with
        | false ->
            match x |> Array.isEmpty with
            | false -> x |> hash' prime1 prime2 |> succeed
            | true -> Empty |> fail
        | true -> Null |> fail
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
            | true -> Null |> fail

module StringHelpers =
    open System

    let isNullOrEmpty = String.IsNullOrEmpty
    let isNullOrWhiteSpace = String.IsNullOrWhiteSpace