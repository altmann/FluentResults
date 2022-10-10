namespace FluentResults.Samples.WebHost.Controllers
{
    public class MaxLengthError : Error
    {
        public MaxLengthError(string propertyName, int maxLength)
            : base($"'{propertyName}' should have a max length of {maxLength} characters.")
        { }
    }

    public class UnauthorizedError : Error
    {
        public UnauthorizedError()
            : base("Unauthorized")
        { }
    }

    public class NotFoundError : Error
    {
        public NotFoundError(string entityName, Guid id)
            : base($"'{entityName}' with id '{id}' not found.")
        { }
    }
}
