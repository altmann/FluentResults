<img src="https://github.com/altmann/FluentResults/blob/master/resources/icons/FluentResults-Icon-64.png" alt="FluentResults"/>

# FluentResults

[![Nuget downloads](https://img.shields.io/nuget/v/fluentresults.svg)](https://www.nuget.org/packages/FluentResults/)
[![Nuget](https://img.shields.io/nuget/dt/fluentresults)](https://www.nuget.org/packages/FluentResults/)
[![Build status](https://dev.azure.com/altmann/FluentResults/_apis/build/status/FluentResults-CI)](https://dev.azure.com/altmann/FluentResults/_build/latest?definitionId=11)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/altmann/FluentResults/blob/master/LICENSE)

FluentResults is a lightweight .NET library built to solve a common problem - returning an object indicates success or failure of an operation instead of using exceptions. 

You should install [FluentResults with NuGet](https://www.nuget.org/packages/FluentResults/):

```
Install-Package FluentResults
```

## Key Features

- Storing multiple errors in one Result object
- Storing powerful Error and Success objects and not only error string messages
- Designing Errors/Success in an object-oriented way
- Storing the root cause chain of errors in a hierarchical way
- Provide 
  - .NET Standard, .NET Core and .NET Full Framework support
  - SourceLink support
  - powerful [code samples](https://github.com/altmann/FluentResults#samplesbest-practices) which show the integration with famous or common frameworks/libraries

## Why Results instead of exceptions

To be honest, returning a Result object indicates success or failure is not a new idea. Martin Fowler already wrote about this pattern  in 2004.

If you want to know what Martin Fowler think about Results read that: [Notification by Martin Fowler](https://martinfowler.com/eaaDev/Notification.html)

If you want a second opinion read that: [Error handling: Exception or Result? by Vladimir Khorikov](http://enterprisecraftsmanship.com/2017/03/13/error-handling-exception-or-result/)

If you want a short summary read that: [Error Handling — Returning Results by Michael Altmann](https://medium.com/@michael_altmann/error-handling-returning-results-2b88b5ea11e9)

## Creating a Result

A Result can store multiple Error and Success messages.

```csharp
// create a result which indicates success
Result successResult1 = Result.Ok();

// create a result which indicates failure
Result errorResult1 = Result.Fail("My error message");
Result errorResult2 = Result.Fail(new StartDateIsAfterEndDateError(startDate, endDate));
```
    
The class `Result` is typically used by void methods which have no return value.

```csharp
public Result DoTask()
{
    if (this.State == TaskState.Done)
        return Result.Fail("Task is in the wrong state.");

    // rest of the logic

    return Result.Ok();
}
```

Additionally a value from a specific type can also be stored if necessary.

```csharp
// create a result which indicates success
Result<int> successResult1 = Result.Ok(42);
Result<MyCustomObject> successResult2 = Result.Ok(new MyCustomObject());

// create a result which indicates failure
Result<int> errorResult = Result.Fail<int>("My error message");
```

The class `Result<T>` is typically used by methods with a return type. 

```csharp
public Result<Task> GetTask()
{
    if (this.State == TaskState.Deleted)
        return Result.Fail<Task>("Deleted Tasks can not be displayed.");

    // rest of the logic

    return Result.Ok(task);
}
```

## Processing a Result

After you get a Result object from a method you have to process it. This means, you have to check if the operation completed successfully or not. The properties `IsSuccess` and `IsFailed` at the Result object indicates success or failure. The value of a `Result<T>` can be accessed via the properties `Value` and `ValueOrDefault`.

```csharp
Result<int> result = DoSomething();
     
// get all reasons why result object indicates success or failure. 
// contains Error and Success messages
IEnumerable<Reason> reasons = result.Reasons;

// get all Error messages
IEnumerable<Error> errors = result.Errors;

// get all Success messages
IEnumerable<Success> successes = result.Successes;

if (result.IsFailed)
{
    // handle error case
    var value1 = result.Value; // throws exception because result is in failed state
    var value2 = result.ValueOrDefault; // return default value (=0) because result is in failed state
    return;
}

// handle success case
var value3 = result.Value; // return value and doesn't throw exception because result is in success state
var value4 = result.ValueOrDefault; // return value because result is in success state
```

## Designing errors and success messages

There are many Result Libraries which stores only simple string messages. FluentResults stores instead powerful object-oriented Error and Success objects. The advantage is that all relevant information of an error is encapsulated within one class. 

The classes `Success` and `Error` inherit from the base class `Reason`. If at least one `Error` object is in the `Reasons` property then the result indicates a failure and the property `IsSuccess` is false. 

You can create your own `Success` or `Error` classes when you inherit from `Success` or `Error`. 

```csharp
public class StartDateIsAfterEndDateError : Error
{
    public StartDateIsAfterEndDateError(DateTime startDate, DateTime endDate)
        : base($"The start date {startDate} is after the end date {endDate}")
    { 
        Metadata.Add("ErrorCode", "12");
    }
}
```

With this mechanism you can also create a class `Warning`. You can choose if a Warning in your system indicates a success or a failure by inheriting from `Success` or `Error` classes.  

## Further features

### Chaining error and success messages

In some cases it is necessary to chain multiple error and success messages in one result object. 

```csharp
var result = Result.Fail("error message 1")
                    .WithError("error message 2")
                    .WithError("error message 3")
                    .WithSuccess("success message 1");
```
	
### Root cause of the error

You can also store the root cause of the error in the error object.

```csharp
try
{
    //export csv file
}
catch(CsvExportException ex)
{
    return Result.Fail(new Error("CSV Export not executed successfully")
                        .CausedBy(ex));
}
```

### Metadata

It is possible to add metadata to Error or Success objects. 

One way doing that is to call the method `WithMetadata(...)` directly at the creation of the result object. 

```csharp
var result1 = Result.Fail(new Error("Error 1")
                          .WithMetadata("metadata name", "metadata value"));

var result2 = Result.Ok()
                    .WithSuccess(new Success("Success 1")
                                 .WithMetadata("metadata name", "metadata value"));
```

Another way is to call `WithMetadata(...)` in constructor of the Error or Success class. 

```csharp
public class DomainError : Error
{
    public DomainError(string message)
        : base(message)
    { 
        WithMetadata("ErrorCode", "12");
    }
}
```

### Merging

Multiple results can be merged with the static method `Merge()`.

```csharp
var result1 = Result.Ok();
var result2 = Result.Fail("first error");
var result3 = Result.Ok<int>();

var mergedResult = Result.Merge(result1, result2, result3);
```

### Converting

A result object can be converted to another result object with the methods `ToResult()` and `ToResult<TValue>()`.

```csharp
Result.Ok().ToResult<int>(); // converting a result to a result from type Result<int>
Result.Ok<int>(5).ToResult<float>(v => v); // converting a result to a result from type Result<float>
Result.Fail<int>("Failed").ToResult<float>() // converting a result from type Result<int> to result from type Result<float> without passing the converting logic because result is in failed state and therefore no converting logic needed
Result.Ok<int>().ToResult(); // converting a result to a result from type Result
```

### Handling/catching errors

Similar to the catch block for exceptions the checking and handling of errors within a Result object is supported by this library with some methods: 

```csharp
result.HasError<MyCustomError>(); // check if the Result object contains an error from a specific type
result.HasError<MyCustomError>(myCustomError => myCustomError.MyField == 2); // check if the Result object contains an error from a specific type and with a specific condition
result.HasError(error => error.HasMetadataKey("MyKey")); // check if the Result object contains an error with a specific metadata key
result.HasError(error => error.HasMetadata("MyKey", metadataValue => (string)metadataValue == "MyValue")); // check if the Result object contains an error with a specific metadata
```

### Handling successes

Checking if a result object contains a specific success object can be done with the method `HasSuccess()`

```csharp
result.HasSuccess<MyCustomSuccess>(); // check if the Result object contains a success from a specific type
result.HasSuccess<MyCustomSuccess>(success => success.MyField == 3); // check if the Result object contains a success from a specific type and with a specific condition
```

### Logging

Sometimes it is necessary to log results. First create a logger. 

```csharp
public class MyConsoleLogger : IResultLogger
{
    public void Log(string context, ResultBase result)
    {
        Console.WriteLine("{0}", result);
    }
}
```

Then you have to register your logger. 

```csharp
var myLogger = new MyConsoleLogger();
        Result.Setup(cfg => {
            cfg.Logger = myLogger;
        });
```

Finally the logger can be used. 

```csharp
var result = Result.Fail("Operation failed")
    .Log();
```

Additionally a context as string can be passed.

```csharp
var result = Result.Fail("Operation failed")
    .Log("logger context");
```

## Samples/Best Practices

Here are some samples and best practices using FluentResult or the Result pattern in general with some famous or common used frameworks and libraries.

### [MediatR](https://github.com/jbogard/MediatR) request handlers returning Result objects

- [Full functional .NET Core sample code with commands/queries and a ValidationPipelineBehavior](https://github.com/altmann/FluentResults/tree/master/src/FluentResults.Samples.MediatR)
- Return business validation errors via a Result object from a MediatR request handler back to the consumer
- Don't throw exceptions based on business validation errors
- Inject command and query validation via MediatR PipelineBehavior and return a Result object instead of throwing an exception

## Copyright

Copyright (c) Michael Altmann. See [LICENSE](https://raw.githubusercontent.com/altmann/FluentResults/master/LICENSE) for details.
