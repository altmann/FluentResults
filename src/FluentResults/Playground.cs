using System;

namespace FluentResults
{
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
                .WithError(new MyError().CausedBy(new InvalidCastException()));


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

        public void Customized()
        {
            var result = Results<MyValueResult>.Ok()
                .WithSuccess("success message")
                .WithValue(5);
            
            result = Results<MyValueResult>.Fail(new Error("first error"))
                .WithError("second error")
                .WithError<MyError>();

            result = Results<MyValueResult>.Fail("first error");
        }

        public void TestExtensions()
        {
            var result = 5.ToResult();
            var result2 = 5.ToResult<MyValueResult, int>();
        }

        public void MergeTest()
        {
            var result1 = Results.Ok();
            var result2 = Results.Fail("first error");
            var result3 = Results.Ok<int>();

            var mergedResult1 = Results.Merge(result1, result2, result3);
            var convertedResult1 = mergedResult1.ConvertToResultWithValueType<int>();

            var mergedResult2 = Results.Merge<int>(result1, result2, result3)
                .WithValue(5);
            var convertedResult2 = mergedResult2.ConvertTo();

            var mergedResult3 = Results<MyValueResult>.Merge(result1, result2, result3);
            var convertedResult3 = mergedResult3.ConvertTo();
            var convertedResult4 = mergedResult3.ConvertToResultWithValueType<float>();

            var convertedResult = mergedResult3.ConvertToResultWithValueType<Result>();
        }

        public void LogTest()
        {
            var result1 = Results.Ok();
            result1 = result1.Log();
        }
    }
}