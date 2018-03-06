using System;

namespace FluentResults
{
    public class CustomError : Error
    {
        public CustomError()
        {
            Message = "Custom message";
        }
    }

    public class Playground
    {
        public void Simple()
        {
            var voidResult = Results.Ok();
            
            voidResult = Results.Ok()
                .WithSuccess("This is a success")
                .WithSuccess("This is a second success");

            voidResult = Results.Fail("First error");

            voidResult = Results.Fail(new Error("First error"));

            voidResult = Results.Fail("First error")
                .WithError("second error")
                .WithError(new CustomError().CausedBy(new InvalidCastException()));


            var valueResult = Results.Ok<int>()
                .WithSuccess("first success")
                .WithValue(3);

            valueResult = Results.Ok(3);

            valueResult = Results.Ok<int>()
                .WithValue(3);

            valueResult = Results.Fail<int>("First error");

            valueResult = Results.Fail<int>(new Error("first error"))
                .WithError("second error");
        }

        public void TestExtensions()
        {
            var result = 5.ToResult();
        }

        public void MergeTest()
        {
            var result1 = Results.Ok();
            var result2 = Results.Fail("first error");
            var result3 = Results.Ok<int>();

            var mergedResult1 = Results.Merge(result1, result2, result3);
            var convertedResult1 = mergedResult1.ToResult<int>();

            Results.Ok().ToResult<int>();
            Results.Ok<int>().ToResult<float>();
            Results.Ok<int>().ToResult();

            var mergedResult2 = Results.Merge<int>(result1, result2, result3)
                .WithValue(5);

            var convertedResult2 = mergedResult2.ToResult();
        }

        public void LogTest()
        {
            var result1 = Results.Ok();
            result1 = result1.Log();
        }
    }
}