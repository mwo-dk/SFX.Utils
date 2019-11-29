# SFX.Utils

Basic helper routines. The repository contains packages for F# as well as C#, and are exposed in the nuget packages [SFX.Utils](https://www.nuget.org/packages/SFX.Utils/)
and [SFX.Utils.CSharp](https://www.nuget.org/packages/SFX.Utils.CSharp/) respectively.

## Usage C#

The C# library is basically a small collection of helper classes and interfaces (neat when using then in a DI context and faking etc), and deal can be grouped into three scopes:

* Time. Helpers to work with actual time and timers.
* Initialization. Helper interfaces and a single class to let instances initialize themselves or others.
* Hash code

### Time

This small collection, deals with date time and timers.

#### (I)DateTimeProvider

The ```IDateTimeProvider``` is a simple interface, that can deliver the current ```DateTimeOffset``` in UTC:

``` csharp
public interface IDateTimeProvider
{
    DateTimeOffset GetUtcNow();
}
```

Not the hardest interface, and even the youngest develper can figure out it's implementation. It's done many other places and in other ways, searching faking time or asking Rx how to deal with it, gives a lot of good and more advanced options. I like it, because it's simple to fake it in tests.

#### (I)Timer and (I)TimerProvider

Similar to ```IDateTimeProvider``` an abstraction over timers and providers thereof have been provided in:

``` csharp
public interface ITimer : IDisposable
{
    void Start();
    void Stop();
}

public interface ITimerProvider
{
    ITimer Create(TimeSpan interval, Action handler, bool autoStart);
}
```

Again very simple. Testable, and the implementation provided in ```Timer``` and ```TimerProvider``` rely on ```System.Threading.Timer```, which is good in servers, but the other timers available can be implemented for as well.

### Initialization

The notion of post 'construction' initialization is convenient, ie in scenarios where you hook up a large application and then want to start things - throwing exceptions upon object initialization (object construction) or even to heavy processing is a mess there. Then it is simpler to hook up things in a manner:

``` csharp
using static System.Threading.Interlocked;

public sealed class SomeAggregatedService : IInitializable 
{
    private static readonly ObjectName = typeof(SomeAggregatedService).FullName;

    public SomeAggregatedService(ISomeDependency someDependency, IAnotherDependency anotherDependency) =>
        (SomeDependency, AnotherDependency) = 
            (someDependency ?? throw new ArgumentNullException(nameof(someDependency), anotherdependency ?? throw new ArgumentNullException(nameof(anotherDependency))));
    
    internal ISomeDependency SomeDependency {get;} // Internal - easier to test
    internal IAnotherDependency AnotherDependency {get;}

    internal long IsInitializationRunningCount = 0L;
    internal long IsInitializedCount = 0L;
    public void Initialize() 
    {
        try 
        {
            if (1L < Increment(ref IsInitializationRunningCount) || IsInitialized())
                return;

            // Do the heavy lifting...

            Increment(ref IsInitializedCount);
        }
        finally 
        {
            Decrement(ref IsInitializationRunningCount);
        }
    }
    public bool IsInitialized() => 0L < Read(ref IsInitializedCount);

    public void ServiceSomeOne() 
    {
        if (!IsInitialized())
            throw new InvalidOperationException($"{ObjectName} is not initialized");

        // Do the servicing
    }
}
``` 

This is where the interfaces:

``` csharp
public interface IInitializable
{
    void Initialize();
    bool IsInitialized();
}
public interface IAsyncInitializable
{
    Task InitializeAsync();
    bool IsInitialized();
}
``` 

come in handy, wrt wiring up the monolith. Similarly interfaces for wiring up others have been provided:

``` csharp
public interface IInitializer
{
    void Initialize();
}
public interface IAsyncInitializer
{
    Task InitializeAsync();
}
```
### Hash codes

The static class ```SFX.Utils.Infrastructure.HashCodeHelpers``` is just a simple wrapper for doing the standard way of computing the hashcode of complex objects utilizing the primes 19 and 31. The methods are:

* ```ComputeHashCode(this int[] items) -> int```
* ```ComputeHashCode<T>(this T[] items, Func<T, int> getHash) -> int```
* ```ComputeHashCode<T>(this IEnumerable<T> items, Func<T, int> getHash) -> int```
* ```ComputeHashCodeForObjectArray(params object[] items) -> int```

It should be fairly self-explanatory. Why provide a library for this? Reason is, that I myself often have been working with large in memory caches where ie. the key's have been fairly complex, so:

* Pre-calculating the hash of these (immutable) objects upon (maybe) construction, returning that value in ```GetHashCode()``` and then 
* Override ```IEquatable<>``` makes doing this convenient.

## Usage F#

The F# library delivers a few modules that essentially mostly wrap the C# library.

### SFX.Utils.StringHelpers
f
### SFX.Utils.HashHelpers
d
### SFX.Utils.DateTime
f
### SFX.Utils.Timer
f
### SFX.Utils.Initializable
