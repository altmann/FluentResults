namespace FluentResults.Immutable.Extensions;

public static class ResultExtensions
{
    public static Result<T> Bind<T>(
        this Result<Unit> result,
        Func<Result<T>> bindingFunction) =>
        result.IsSuccess && bindingFunction() is var bind
            ? bind with
            {
                Reasons = result.Reasons.AddRange(bind.Reasons),
            }
            : new()
            {
                Reasons = result.Reasons,
            };

    public static async Task<Result<T>> Bind<T>(
        this Result<Unit> result,
        Func<Task<Result<T>>> bindingAsyncFunction) =>
        result.IsSuccess && await bindingAsyncFunction() is var bind
            ? bind with
            {
                Reasons = result.Reasons.AddRange(bind.Reasons),
            }
            : new()
            {
                Reasons = result.Reasons,
            };

    public static async ValueTask<Result<T>> Bind<T>(
        this Result<Unit> result,
        Func<ValueTask<Result<T>>> bindingAsyncFunction) =>
        result.IsSuccess && await bindingAsyncFunction() is var bind
            ? bind with
            {
                Reasons = result.Reasons.AddRange(bind.Reasons),
            }
            : new()
            {
                Reasons = result.Reasons,
            };
}