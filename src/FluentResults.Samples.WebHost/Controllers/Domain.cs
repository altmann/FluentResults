namespace FluentResults.Samples.WebHost.Controllers;

public static class Domain
{
    public static Task<Result> DomainCommandAsync(string failureType)
    {
        return Task.FromResult(DomainCommand(failureType));
    }

    public static Result DomainCommand(string failureType)
    {
        if (failureType.ToLower() == "maxlength")
        {
            return Result.Fail(new MaxLengthError("myProperty", 10));
        }
        if (failureType.ToLower() == "unauthorized")
        {
            return Result.Fail(new UnauthorizedError("altmann", "usermgmt"));
        }
        if (failureType.ToLower() == "notfound")
        {
            return Result.Fail(new NotFoundError("myEntity", Guid.NewGuid()));
        }

        return Result.Ok();
    }

    public static Task<Result<int>> DomainQueryAsync(string failureType)
    {
        return Task.FromResult(DomainQuery(failureType));
    }

    public static Result<int> DomainQuery(string failureType)
    {
        if (failureType.ToLower() == "maxlength")
        {
            return Result.Fail<int>(new MaxLengthError("myProperty", 10));
        }
        if (failureType.ToLower() == "unauthorized")
        {
            return Result.Fail<int>(new UnauthorizedError("altmann", "usermgmt"));
        }
        if (failureType.ToLower() == "notfound")
        {
            return Result.Fail<int>(new NotFoundError("myEntity", Guid.NewGuid()));
        }

        return Result.Ok(12);
    }
}