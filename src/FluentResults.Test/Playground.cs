using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentResults.Extensions;

namespace FluentResults.Test
{
    public class CustomError : Error
    {
        public CustomError() : base("Custom message")
        {
        }

        public CustomError(string message) : base(message)
        {
        }
    }

    public class Playground
    {
        public void Simple()
        {
            var voidResult = Result.Ok();

            voidResult = Result.Ok()
                               .WithSuccess("This is a success")
                               .WithSuccess("This is a second success");

            voidResult = Result.Fail("First error");

            voidResult = Result.Fail(new Error("First error"));

            voidResult = Result.Fail("First error")
                               .WithError("second error")
                               .WithError(new CustomError().CausedBy(new InvalidCastException()));

            var valueResult = Result.Ok<int>(default)
                                    .WithSuccess("first success")
                                    .WithValue(3);

            valueResult = Result.Ok(3);

            valueResult = Result.Ok<int>(default)
                                .WithValue(3);

            valueResult = Result.Fail<int>("First error");

            valueResult = Result.Fail<int>(new Error("first error"))
                                .WithError("second error");

            IEnumerable<Result> results = new List<Result>();
            Result mergedResult = results.Merge();

            IEnumerable<Result<int>> results2 = new List<Result<int>>();
            Result<IEnumerable<int>> mergedResult2 = results.Merge();

            var (isSuccess, isFailed) = Result.Ok();
            var (isSuccess0, isFailed0, errors0) = Result.Ok();
            var (isSuccess1, isFailed1, value1) = Result.Ok(500);
            var (isSuccess2, _, value2) = Result.Ok(500);
            var (isSuccess3, _, errors3) = Result.Fail("First error");
            var (isSuccess4, _, value4, errors4) = Result.Fail<int>("First error");
        }

        public void TestExtensions()
        {
            var result = 5.ToResult();
        }

        public void MergeTest()
        {
            var result1 = Result.Ok();
            var result2 = Result.Fail("first error");
            var result3 = Result.Ok<int>(default);

            var mergedResult1 = Result.Merge(result1, result2, result3);
            var convertedResult1 = mergedResult1.ToResult<int>();

            Result.Ok().ToResult<int>();
            Result.Ok<int>(default).ToResult<float>();
            Result.Ok<int>(default).ToResult();

            var mergedResult2 = Result.Merge(result1, result2, result3);

            var convertedResult2 = mergedResult2.ToResult();
        }

        public void LogTest()
        {
            var result1 = Result.Ok();
            result1 = result1.Log();
        }

        public async Task BindTests()
        {
            var r = Result.Ok(1);

            var r1 = await r.Bind(v => GetVResult())
                            .Bind(v => GetVResultAsync())
                            .Bind(v => GetVResult())
                            .Bind(v => GetVResultAsync())
                ;

            var r2 = await r.Bind(v => GetVResultAsync());
        }

        private Result GetResult() => Result.Ok();

        private Result<int> GetVResult() => Result.Ok(1);

        private Task<Result> GetResultAsync() => Task.FromResult(GetResult());

        private Task<Result<int>> GetVResultAsync() => Task.FromResult(GetVResult());
    }
}