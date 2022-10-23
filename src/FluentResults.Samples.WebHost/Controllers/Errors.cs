namespace FluentResults.Samples.WebHost.Controllers
{
    public abstract class DomainError : Error
    {
        public string ErrorCode { get; }

        protected DomainError(string message, string errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }
    }

    public class MaxLengthError : DomainError
    {
        public string PropertyName { get; }

        public int MaxLength { get; }

        public MaxLengthError(string propertyName, int maxLength)
            : base($"'{propertyName}' should have a max length of {maxLength} characters.", "100")
        {
            PropertyName = propertyName;
            MaxLength = maxLength;
        }
    }

    public class InvalidBirthdateError : DomainError
    {
        public InvalidBirthdateError()
            : base("Person should be 18 or older", "200")
        { }
    }

    public class UnauthorizedError : Error
    {
        public string Username { get; }

        public string Ressource { get; }

        public UnauthorizedError(string username, string ressource)
            : base($"{username} is not authorized to access {ressource}.")
        {
            Username = username;
            Ressource = ressource;
        }
    }

    public class NotFoundError : Error
    {
        public string EntityName { get; }

        public Guid Id { get; }

        public NotFoundError(string entityName, Guid id)
            : base($"'{entityName}' with id '{id}' not found.")
        {
            EntityName = entityName;
            Id = id;
        }
    }
}