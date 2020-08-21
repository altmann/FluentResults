namespace FluentResults.Samples.WebController
{
    // This is an ASP.NET WebApi Controller
    public class CustomerController
    {
        public ResultDto CreateCustomer(CreateCustomerDto model)
        {
            var result = CreateCustomerInternal(model);

            // Use an custom ResultDto class at the system boundaries so that the public api is in your responsibility.
            // Transform the FluentResult Result object to an custom ResultDto object as last as possible.
            return result.ToResultDto();
        }

        private Result CreateCustomerInternal(CreateCustomerDto model)
        {
            // Create customer business logic
            return Result.Ok();
        }
    }

    public class CreateCustomerDto
    {
        // Some customer properties
    }
}