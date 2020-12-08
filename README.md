<img src="https://github.com/altmann/FluentResults/blob/master/resources/icons/FluentResults-Icon-64.png" alt="FluentResults"/>

# FluentResults

[![Nuget downloads](https://img.shields.io/nuget/v/fluentresults.svg)](https://www.nuget.org/packages/FluentResults/)
[![Nuget](https://img.shields.io/nuget/dt/fluentresults)](https://www.nuget.org/packages/FluentResults/)
[![Build status](https://dev.azure.com/altmann/FluentResults/_apis/build/status/FluentResults-CI)](https://dev.azure.com/altmann/FluentResults/_build/latest?definitionId=11)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/altmann/FluentResults/blob/master/LICENSE)

**FluentResults is a lightweight .NET library developed to solve a common problem. It returns an object indicating success or failure of an operation instead of throwing/using exceptions.**

You can install [FluentResults with NuGet](https://www.nuget.org/packages/FluentResults/):

```
Install-Package FluentResults
```

## Key Features

- **Generalised container** which works in all contexts (ASP.NET MVC/WebApi, WPF, DDD Domain Model, etc)
- Store **multiple errors** in one Result object
- Store **powerful and elaborative Error and Success objects** instead of only error messages in string format
- Designing Errors/Success in an object-oriented way
- Store the **root cause with chain of errors in a hierarchical way**
- Provide 
  - .NET Standard, .NET Core and .NET Full Framework support
  - SourceLink support
  - powerful [code samples](https://github.com/altmann/FluentResults#samplesbest-practices) which show the integration with famous or common frameworks/libraries

## Why Results instead of exceptions

To be honest, the pattern - returning a Result object indicating success or failure - is not at all a new idea. This pattern comes from functional programming languages. With FluentResults this pattern can also be applied in .NET/C#. 

The article [Exceptions for Flow Control by Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/exceptions-for-flow-control/) describes very good in which scenarios the Result pattern makes sense and in which not. See the [list of Best Practices](https://github.com/altmann/FluentResults#samplesbest-practices) and the [list of resources](https://github.com/altmann/FluentResults#interesting-resources-about-result-pattern) to learn more about the Result Pattern.

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

After you get a Result object from a method you have to process it. This means, you have to check if the operation was completed successfully or not. The properties `IsSuccess` and `IsFailed` in the Result object indicate success or failure. The value of a `Result<T>` can be accessed via the properties `Value` and `ValueOrDefault`.

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

There are many Result Libraries which store only simple string messages. FluentResults instead stores powerful object-oriented Error and Success objects. The advantage is all the relevant information of an error is encapsulated within one class. 

The classes `Success` and `Error` inherit from the base class `Reason`. If at least one `Error` object exists in the `Reasons` property then the result indicates a failure and the property `IsSuccess` is false. 

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

### Create a result depending on success/failure condition

Very often you have to create a fail or success result depending on a condition. Usually you can write it in this way:

```csharp
var result = string.IsNullOrEmpty(firstName) ? Result.Fail("First Name is empty") : Result.Ok();
```

With the methods ```FailIf()``` and ```OkIf()``` you can also write in a more readable way:

```csharp
var result = Result.FailIf(string.IsNullOrEmpty(firstName), "First Name is empty");
```

### Try

In some scenarios you want to execute an action. If this action throws an exception then the exception should be catched and transformed to a result object. 

```csharp
var result = Result.Try(() => DoSomethingCritical());
```

In the above example the default catchHandler is used. The behavior of the default catchHandler can be overwritten via the global Result settings (see next example). You can control how the Error object looks.

```csharp
Result.Setup(cfg =>
{
    cfg.DefaultTryCatchHandler = exception =>
    {
        if (exception is SqlTypeException sqlException)
            return new ExceptionalError("Sql Fehler", sqlException);

        if (exception is DomainException domainException)
            return new Error("Domain Fehler")
                .CausedBy(new ExceptionError(domainException.Message, domainException));

        return new Error(exception.Message);
    };
});

var result = Result.Try(() => DoSomethingCritical());
```

It is also possible to pass a custom catchHandler via the ```Try(..)``` method. 

```csharp
var result = Result.Try(() => DoSomethingCritical(), ex => new MyCustomExceptionError(ex));
```

### Root cause of the error

You can also store the root cause of the error in the error object. With the method `CausedBy(...)` the root cause can be passed as Error, list of Errors, string, list of strings or as exception. The root cause is stored in the `Reasons` property of the error object. 

Example 1 - root cause is an exception
```csharp
try
{
    //export csv file
}
catch(CsvExportException ex)
{
    return Result.Fail(new Error("CSV Export not executed successfully").CausedBy(ex));
}
```

Example 2 - root cause is an error
```csharp
Error rootCauseError = new Error("This is the root cause of the error");
Result result = Result.Fail(new Error("Do something failed", rootCauseError));
```

Example 3 - reading root cause from errors
```csharp
Result result = ....;
if (result.IsSuccess)
   return;

foreach(var error in result.Errors)
{
    foreach(var causedByExceptionalError in error.Reasons.OfType<ExceptionalError>())
    {
        Console.WriteLine(causedByExceptionalError.Exception);
    }
}
```

### Metadata

It is possible to add metadata to Error or Success objects. 

One way of doing that is to call the method `WithMetadata(...)` directly where result object is being created. 

```csharp
var result1 = Result.Fail(new Error("Error 1").WithMetadata("metadata name", "metadata value"));

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

A result object can be converted to another result object with methods `ToResult()` and `ToResult<TValue>()`.

```csharp
// converting a result to a result from type Result<int>
Result.Ok().ToResult<int>();

// converting a result to a result from type Result<float>
Result.Ok<int>(5).ToResult<float>(v => v);

// converting a result from type Result<int> to result from type Result<float> without passing the converting logic because result is in failed state and therefore no converting logic needed
Result.Fail<int>("Failed").ToResult<float>();

// converting a result to a result from type Result
Result.Ok<int>().ToResult(); 
```

### Handling/catching errors

Similar to the catch block for exceptions, the checking and handling of errors within Result object is also supported using some methods: 

```csharp
// check if the Result object contains an error from a specific type
result.HasError<MyCustomError>();

// check if the Result object contains an error from a specific type and with a specific condition
result.HasError<MyCustomError>(myCustomError => myCustomError.MyField == 2);

// check if the Result object contains an error with a specific metadata key
result.HasError(error => error.HasMetadataKey("MyKey"));

// check if the Result object contains an error with a specific metadata
result.HasError(error => error.HasMetadata("MyKey", metadataValue => (string)metadataValue == "MyValue")); 
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

Additionally, a context can be passed in form of a string.

```csharp
var result = Result.Fail("Operation failed")
    .Log("logger context");
```

## Samples/Best Practices

Here are some samples and best practices to be followed while using FluentResult or the Result pattern in general with some famous or commonly used frameworks and libraries.

### Powerful domain model inspired by Domain Driven Design

- [Domain model with a command handler](https://github.com/altmann/FluentResults/tree/master/src/FluentResults.Samples/DomainDrivenDesign)
- Protecting domain invariants by using for example factory methods returning a Result object
- Make each error unique by making your own custom Error classes inheriting from Error class
- If the method doesn't have a failure scenario then don't use the Result class as return type
- Be aware that you can merge multiple failed results or return the first failed result asap

### Serializing Result objects (ASP.NET WebApi, [Hangfire](https://www.hangfire.io/))

- [Asp.net WebController](https://github.com/altmann/FluentResults/tree/master/src/FluentResults.Samples/WebController)
- [Hangfire Job](https://github.com/altmann/FluentResults/tree/master/src/FluentResults.Samples/HangfireJobs)
- Don't serialize FluentResult result objects. 
- Make your own custom ResultDto class for your public api in your system boundaries
  - So you can control which data is submitted and which data is serialized
  - Your public api is independent of third party libraries like FluentResults
  - You can keep your public api stable

### [MediatR](https://github.com/jbogard/MediatR) request handlers returning Result objects

- [Full functional .NET Core sample code with commands/queries and a ValidationPipelineBehavior](https://github.com/altmann/FluentResults/tree/master/src/FluentResults.Samples.MediatR)
- Returns business validation errors via a Result object from a MediatR request handler back to the consumer
- Don't throw exceptions based on business validation errors
- Inject command and query validation via MediatR PipelineBehavior and return a Result object instead of throwing an exception

## Interesting Resources about Result Pattern

- [Error Handling — Returning Results by Michael Altmann](https://medium.com/@michael_altmann/error-handling-returning-results-2b88b5ea11e9)
- [Operation Result Pattern by Carl-Hugo Marcotte](https://www.forevolve.com/en/articles/2018/03/19/operation-result/)
- [Exceptions for flow control in C# by Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/exceptions-for-flow-control/)
- [Error handling: Exception or Result? by Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/error-handling-exception-or-result/)
- [What is an exceptional situation in code? by Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/what-is-exceptional-situation/)
- [Advanced error handling techniques by Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/advanced-error-handling-techniques/)
- [A Simple Guide by Isaac Cummings](https://medium.com/@cummingsi1993/the-operation-result-pattern-a-simple-guide-fe10ff959080)
- [Flexible Error Handling w/ the Result Class by Khalil Stemmler](https://khalilstemmler.com/articles/enterprise-typescript-nodejs/handling-errors-result-class/)
- [Combining ASP.NET Core validation attributes with Value Objects by Vladimir Khorikov](https://enterprisecraftsmanship.com/posts/combining-asp-net-core-attributes-with-value-objects/)

## Contributors

Thanks to all the contributers and to all the people who gave feedback!

- [deepankkartikey](https://github.com/deepankkartikey)
- [Loreno10](https://github.com/Loreno10)
- [JasonLandbridge](https://github.com/JasonLandbridge)
- [michaelmsm89](https://github.com/michaelmsm89)

## Copyright

Copyright (c) Michael Altmann. See [LICENSE](https://raw.githubusercontent.com/altmann/FluentResults/master/LICENSE) for details.
