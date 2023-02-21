using FsCheck;
using FsCheck.Xunit;

namespace FluentResults.Immutable.Tests;

public sealed class OptionTests
{
    [Property(DisplayName = "Some has appropriate value", MaxTest = 1000)]
    public Property SomeHasAdequateValue() =>
        Prop.ForAll(
            Arb.Generate<int>()
                .Select(static i => (Option: Option.Some(i), ExpectedValue: i))
                .ToArbitrary(),
            static tuple =>
            {
                var (option, expectedValue) = tuple;

                return option is { Value: var actualValue, } &&
                    actualValue == expectedValue;
            });

    [Property(DisplayName = "Some can be converted using with operator", MaxTest = 1000)]
    public Property SomeCanBeConvertedUsingWithOperator() =>
        Prop.ForAll(
            Gen.Two(Arb.Generate<int>())
                .Where(static tuple => tuple.Item1 != tuple.Item2)
                .Select(static tuple => (Option: Option.Some(tuple.Item1), NewValue: tuple.Item2))
                .ToArbitrary(),
            static tuple =>
            {
                var (option, newValue) = tuple;

                var newOption = option with
                {
                    Value = newValue,
                };

                return newOption is { Value: var actualValue, } &&
                    actualValue == newValue;
            });

    [Property(DisplayName = "Match of some returns proper value", MaxTest = 1000)]
    public Property MatchOfSomeReturnsProperValue() =>
        Prop.ForAll(
            Arb.Generate<int>()
                .Select(Option.Some)
                .ToArbitrary(),
            static option =>
                option.Match(
                        static _ => true,
                        static () => false)
                    .ToProperty());

    [Property(DisplayName = "Match of none returns proper value", MaxTest = 1000)]
    public Property MatchOfNoneReturnsProperValue() =>
        Prop.ForAll(
            Arb.Generate<int>()
                .Select(static _ => Option.None<int>())
                .ToArbitrary(),
            static option =>
                option.Match(
                        static _ => false,
                        static () => true)
                    .ToProperty());

    [Property(DisplayName = "Match of some executes proper action", MaxTest = 1000)]
    public Property MatchOfSomeExecutesProperAction() =>
        Prop.ForAll(
            Arb.Generate<int>()
                .Select(Option.Some)
                .ToArbitrary(),
            static option =>
            {
                var result = false;

                option.Match(
                    _ => result = true,
                    static () => { });

                return result;
            });

    [Property(DisplayName = "Match of none executes proper action", MaxTest = 1000)]
    public Property MatchOfNoneExecutesProperAction() =>
        Prop.ForAll(
            Arb.Generate<int>()
                .Select(static _ => Option.None<int>())
                .ToArbitrary(),
            static option =>
            {
                var result = false;

                option.Match(
                    static _ => { },
                    () => result = true);

                return result;
            });
}