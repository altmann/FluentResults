namespace FluentResults.Samples.DomainDrivenDesign
{
    // ValueObject
    public class Address
    {
        public string Street { get; private set; }

        public string Number { get; private set; }

        public string City { get; private set; }

        private Address()
        {
        }

        public static Result<Address> Create(string street, string number, string city)
        {
            var validationResult = Result.Merge(
                ValidateStreet(street), 
                ValidateNumber(number));

            // Return failed result if passed parameters are invalid
            if (validationResult.IsFailed)
                return validationResult;

            return Result.Ok(new Address
            {
                Street = street,
                Number = number,
                City = city
            });
        }

        private static Result ValidateNumber(string number)
        {
            // option 1: set error as string message
            return Result.FailIf(() => string.IsNullOrEmpty(number), "Number is required.");

            // option 2: set error as Error object
            return Result.FailIf(() => string.IsNullOrEmpty(number), new Error("Number is required."));

            // option 3: set error as custom Error object
            return Result.FailIf(() => string.IsNullOrEmpty(number), new RequiredError("Number"));
        }

        private static Result ValidateStreet(string street)
        {
            // Multiple results are merged into one result
            return Result.Merge(
                Result.FailIf(() => string.IsNullOrEmpty(street), "Street is required."),
                Result.FailIf(() => !string.IsNullOrEmpty(street) && street.Length > 30,
                    "Street should contain max 30 characters.")
            );
        }
    }

    public class RequiredError : Error
    {
        public string FieldName { get; private set; }
        
        public RequiredError(string fieldName) 
            : base($"{fieldName} is required.")
        {
            FieldName = fieldName;
            Metadata.Add("ErrorCode", 100);
        }
    }
}