<img src="https://github.com/altmann/FluentResults/blob/master/resources/icons/FluentResults-Icon-64.png" alt="FluentResults"/>

# FluentResults

FluentResults is a lightweight .NET library built to solve a common problem - returning an object indicates success or failure of an operation instead of using exceptions. 

You should install [FluentResults with NuGet](https://www.nuget.org/packages/FluentResults/):

    Install-Package FluentResults

## Top Features

- Storing multiple errors in one Result object
- Storing not only error string messages but powerful Error objects
- Designing Errors in a object-oriented way
- Storing the root cause chain of errors in a hierarchical way
- Storing multiple success messages in one Result object
- Provide .NET Standard Support

## Why Results instead of exceptions

To be honest, returning a Result object indicates success or failure is not a new idea. Martin Fowler already wrote about this pattern  in 2004.

If you want to know what Martin Fowler think about Results read that: [Notification by Martin Fowler](https://martinfowler.com/eaaDev/Notification.html)

If you want a second opinion read that: [Error handling: Exception or Result? by Vladimir Khorikov](http://enterprisecraftsmanship.com/2017/03/13/error-handling-exception-or-result/)

If you want a short summary read that: [Error Handling — Returning Results by Michael Altmann](https://medium.com/@michael_altmann/error-handling-returning-results-2b88b5ea11e9)

## Creating a Result
There are two types of Results, a success result and an error result. Both of them can be created easily:

     Result successResult = Results.Ok(); //create a success result
     Result errorResult = Results.Fail("Operation failed"); //create an error result

The class Result can be used for typical void methods which have no return value.

## Processing a Result
After you get a Result object from a method you have to process it. This means, you have to check if the operation completed successfully or not. In other words if the returned Result object is an error or success Result. You can distinguish between success and error results with the properties `IsSuccess` and `IsFailed`.

     Result result = DoSomething();

     if(result.IsFailed)
     {
          // handle error case
          Console.WriteLine(result);
          return;
     }

     //handle success case

## Creating a ValueResult
For methods with return value the generic class `Result<T>` should be used which have a further property `Value` to store a value. You can set the property value in a fluent way.

     Result<int> successResult = Results.Ok<int>(5); //create a success result with Value 5
     Result<int> errorResult = Results.Fail<int>("Operation failed"); //create an error result

## Processing a ValueResult
Processing a ValueResult object is as easy as processing a Result object. You can access the value within the returned ValueResult object via the properties `Value` and `ValueOrDefault`. The property `Value` throws an exception if the ValueResult object is in failed state. The property `ValueOrDefault` return the default value of the value type if the ValueResult is in success state. 

     Result<int> result = DoSomething();

     if(result.IsFailed)
     {
          // handle error case
          Console.WriteLine(result);
          var value1 = result.Value; // throws exception because result is in failed state
          var value2 = result.ValueOrDefault; // return default value (=0) because result is in failed state
          return;
     }

     //handle success case
     var value3 = result.Value; // return value and doesn't throw exception because result is in success state
     var value4 = result.ValueOrDefault; // return value because result is in success state

## Designing errors and success messages

There are many Result Libraries which stores only simple string messages, but FluentResults stores powerful object-oriented Error and Success objects. The advantage is that all relevant information of an error is encapsulated within a class. Here is an example:

    public class StartDateIsAfterEndDateError : Error
    {
        public StartDateIsAfterEndDateError(DateTime startDate, DateTime endDate)
            : base($"The start date {startDate} is after the end date {endDate}")
        { 
            Metadata.Add("ErrorCode", "12");
        }
    }

You can use this error classes with the fluent API of FluentResults. 

    var result = Results.Fail(new StartDateIsAfterEndDateError(startDate, endDate));
    
You can also store the root cause of the error in the error object. 

    try
    {
        //export csv file
    }
    catch(CsvExportException ex)
    {
        return Results.Fail(new Error("CSV Export not executed successfully")
            .CausedBy(ex));
    }

## Further features

### Chaining error and success messages

In some cases it is necessary to chain multiple error and success messages in one result object. 

    var result = Results.Fail("error message 1")
        .WithError("error message 2")
        .WithError("error message 3")
        .WithSuccess("success message 1");
        
### Metadata

It is possible to add metadata to error or success objects. 

    var result1 = Results.Fail(new Error("Error 1").WithMetadata("metadata name", "metadata value"));
    var result2 = Results.Ok()
        .WithSuccess(new Success("Success 1").WithMetadata("metadata name", "metadata value"));

### Merging

Multiple results can be merged with the static method `Merge()`.

    var result1 = Results.Ok();
    var result2 = Results.Fail("first error");
    var result3 = Results.Ok<int>();

    var mergedResult = Results.Merge(result1, result2, result3);

### Converting

A result object can be converted to another result object with the methods `ToResult()` and `ToResult<TValue>()`.

    Results.Ok().ToResult<int>(); // converting a result to a result from type `Result<int>`
    Results.Ok<int>().ToResult<float>(); // converting a result to a result from type `Result<float>`
    Results.Ok<int>().ToResult(); // converting a result to a result from type `Result`

### Logging

Sometimes it is necessary to log results. First create a logger. 

    public class MyConsoleLogger : ILogger
    {
        public void Log(string context, ResultBase result)
        {
            Console.WriteLine("{0}", result);
        }
    }

Then you have to register your logger. 

    var myLogger = new MyConsoleLogger();
            Results.Setup(cfg => {
                cfg.Logger = myLogger;
            });

Finally the logger can be used. 

    var result = Results.Fail("Operation failed")
        .Log();
        
Additionally a context as string can be passed.

    var result = Results.Fail("Operation failed")
        .Log("logger context");

Extension methods
