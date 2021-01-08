using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Common;

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

            var rv = Result.Ok(1);
            rv.Should()
                .BeSuccess()
                .And.HaveValue(5);

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
}
