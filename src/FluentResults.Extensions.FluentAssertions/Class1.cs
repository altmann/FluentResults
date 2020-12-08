using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace FluentResults.Extensions.FluentAssertions
{

    public class Playground
    {
        public class Person
        {

        }

        public void s()
        {
            List<Error> a;

            //a.Should().Contain(e => e.Message == "");

            Dictionary<string, Person> d;

            //d.Should().

            //d.Should().

            Error es;

            Result r = Result.Ok();

            r.Should().BeFailure()
                .And.Satisfy(res =>
                {
                    res.HasError(x => x.Message == "");
                    res.Errors.Should().Contain(e => e.Message == "error");
                    res.Successes.Should().HaveCount(0);
                });


            //r.Should().

            //a.Should().Match(e => true)
            //    .And.

            //es.Should().Match<Error>(error => error.Message == null)
            //    .And.

            //a.Should().Contain
        }
    }

    public static class ResultExtensions
    {
        public static ResultAssertions Should(this Result value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            return new ResultAssertions(value);
        }
    }

    public class ResultAssertions : ReferenceTypeAssertions<Result, ResultAssertions>
    {
        public ResultAssertions(Result instance)
        {
            Subject = instance;
        }

        protected override string Identifier => "result";

        public AndConstraint<ResultAssertions> BeFailure(string because = "")
        {
            Execute.Assertion
                   .BecauseOf(because)
                   .Given(() => Subject.IsFailed)
                   .ForCondition(isFailed => isFailed)
                   .FailWith("Expected result be failed, but is success");

            return new AndConstraint<ResultAssertions>(this);
        }

        public AndConstraint<ResultAssertions> BeSuccess(string because = "")
        {
            Execute.Assertion
                   .BecauseOf(because)
                   .Given(() => Subject.IsSuccess)
                   .ForCondition(isSuccess => isSuccess)
                   .FailWith($"Expected result be success, but is failed because of '{FormatError(Subject.Errors)}'");

            return new AndConstraint<ResultAssertions>(this);
        }

        public AndConstraint<ResultAssertions> Satisfy(Action<Result> action)
        {
            action(Subject);

            return new AndConstraint<ResultAssertions>(this);
        }

        public AndConstraint<ResultAssertions> HasErrorMessage(string expectedErrorMessage, string because = "")
        {
            Execute.Assertion
                   .BecauseOf(because)
                   .ForCondition(Subject.IsFailed)
                   .FailWith("Error message can't asserted because result is not failed")
                   .Then
                   .Given(() => Subject.Errors)
                   .ForCondition(actualErrors => actualErrors.FirstOrDefault(actualError => actualError.Message == expectedErrorMessage) != null)
                   .FailWith($"Expected error message is '{expectedErrorMessage}', but is '{FormatError(Subject.Errors)}'");

            return new AndConstraint<ResultAssertions>(this);
        }

        //public AndConstraint<ResultAssertions> HasErrorMessages(string[] expectedErrorMessages, string because = "")
        //{
        //    Execute.Assertion
        //           .BecauseOf(because)
        //           .ForCondition(Subject.IsFailure)
        //           .FailWith("Error message can't asserted because result is not failed")
        //           .Then
        //           .Given(() => Subject.Error)
        //           .ForCondition(actualErrorMessage =>
        //           {
        //               var actualErrorMessages = ResultUtil.SplitErrorsToList(actualErrorMessage);
        //               return ResultComparer.AreErrorMessagesWithoutMetaInfoEqual(expectedErrorMessages, actualErrorMessages);
        //           })
        //           .FailWith($"Expected error message is '{string.Join(", ", expectedErrorMessages)}', but is '{FormatError(Subject.Error)}'");

        //    return new AndConstraint<ResultAssertions>(this);
        //}

        private static string FormatError(IEnumerable<Error> errors)
        {
            // todo: if no errors
            //var errorMessagesWithMetaInfo = ResultUtil.SplitErrorsToList(allErrorMessages);
            //var errorMessagesWithoutMetaInfo = ResultUtil.RemoveMetaInfo(errorMessagesWithMetaInfo);

            //return string.Join(", ", errorMessagesWithoutMetaInfo);
            return "";
        }
    }
}
