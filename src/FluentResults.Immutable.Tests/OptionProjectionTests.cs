using FsCheck;
using FsCheck.Xunit;

namespace FluentResults.Immutable.Tests;

public class OptionProjectionTests
{
    [Property(DisplayName = "Projecting two options when either is none returns none", MaxTest = 1000)]
    public Property ProjectingTwoOptionsWhenOneOfThemIsNoneReturnsNone() =>
        Prop.ForAll(
            GetOptionalIntegerGenerator()
                .Two()
                .Where(static tuple => tuple.Item1 is None<int> || tuple.Item2 is None<int>)
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second) = tuple;

                return first.SelectMany(_ => second, static (i, i1) => Option.Some(i + i1)) is None<int>;
            });

    [Property(DisplayName = "Projecting three options when either is none returns none", MaxTest = 1000)]
    public Property ProjectingThreeOptionsWhenOneOfThemIsNoneReturnsNone() =>
        Prop.ForAll(
            GetOptionalIntegerGenerator()
                .Three()
                .Where(static tuple => tuple.Item1 is None<int> || tuple.Item2 is None<int> || tuple.Item3 is None<int>)
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third) = tuple;

                return first.SelectMany(
                    _ => second,
                    _ => third,
                    static (
                        i,
                        i1,
                        i2) => Option.Some(i + i1 + i2)) is None<int>;
            });

    [Property(DisplayName = "Projecting four options when either is none returns none", MaxTest = 1000)]
    public Property ProjectingFourOptionsWhenOneOfThemIsNoneReturnsNone() =>
        Prop.ForAll(
            GetOptionalIntegerGenerator()
                .Four()
                .Where(
                    static tuple => tuple.Item1 is None<int> ||
                        tuple.Item2 is None<int> ||
                        tuple.Item3 is None<int> ||
                        tuple.Item4 is None<int>)
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth) = tuple;

                return first.SelectMany(
                    _ => second,
                    _ => third,
                    _ => fourth,
                    static (
                        i,
                        i1,
                        i2,
                        i3) => Option.Some(i + i1 + i2 + i3)) is None<int>;
            });

    [Property(DisplayName = "Projecting five options when either is none returns none", MaxTest = 1000)]
    public Property ProjectingFiveOptionsWhenOneOfThemIsNoneReturnsNone() =>
        Prop.ForAll(
            GetOptionalIntegerGenerator()
                .Four()
                .SelectMany(
                    static _ => GetOptionalIntegerGenerator(),
                    static (tuple, option) => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, option))
                .Where(
                    static tuple => tuple.Item1 is None<int> ||
                        tuple.Item2 is None<int> ||
                        tuple.Item3 is None<int> ||
                        tuple.Item4 is None<int> ||
                        tuple.Item5 is None<int>)
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth, fifth) = tuple;

                return first.SelectMany(
                    _ => second,
                    _ => third,
                    _ => fourth,
                    _ => fifth,
                    static (
                        i,
                        _,
                        _,
                        _,
                        _) => Option.Some(i)) is None<int>;
            });

    [Property(DisplayName = "Projecting six options when either is none returns none", MaxTest = 1000)]
    public Property ProjectingSixOptionsWhenOneOfThemIsNoneReturnsNone() =>
        Prop.ForAll(
            GetOptionalIntegerGenerator()
                .Four()
                .SelectMany(
                    static _ => GetOptionalIntegerGenerator()
                        .Two(),
                    static (tuple, tuple2) => (tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple2.Item1,
                        tuple2.Item2))
                .Where(
                    static tuple => tuple.Item1 is None<int> ||
                        tuple.Item2 is None<int> ||
                        tuple.Item3 is None<int> ||
                        tuple.Item4 is None<int> ||
                        tuple.Item5 is None<int> ||
                        tuple.Item6 is None<int>)
                .ToArbitrary(),
            static tuple =>
            {
                var (first, second, third, fourth, fifth, sixth) = tuple;

                return first.SelectMany(
                    _ => second,
                    _ => third,
                    _ => fourth,
                    _ => fifth,
                    _ => sixth,
                    static (
                        i,
                        _,
                        _,
                        _,
                        _,
                        _) => Option.Some(i)) is None<int>;
            });

    private static Gen<IOption<int>> GetOptionalIntegerGenerator() =>
        Arb.Generate<int>()
            .SelectMany(
                static i => Gen.OneOf(
                    Gen.Fresh<IOption<int>>(() => Option.Some(i)),
                    Gen.Constant<IOption<int>>(Option.None<int>())));
}