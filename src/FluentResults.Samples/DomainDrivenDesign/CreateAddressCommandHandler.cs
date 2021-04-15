namespace FluentResults.Samples.DomainDrivenDesign
{
    public class CreateAddressCommandHandler
    {
        private readonly CustomerRepository _repository;

        public CreateAddressCommandHandler(CustomerRepository repository)
        {
            _repository = repository;
        }

        public Result Handle(CreateAddressCommand command)
        {
            // createAddressResult contains all error messages if address data is invalid
            var createAddressResult = Address.Create(command.Street, command.Number, command.City);

            // Return failed result if address is invalid. Return ASAP
            if (createAddressResult.IsFailed)
                return createAddressResult.ToResult();

            var customer = _repository.GetById(command.CustomerId);

            var addAddressResult = customer.AddAddress(createAddressResult.Value);

            // Return failed result if adding address is not possible because of business rules. Return ASAP
            if (addAddressResult.IsFailed)
                return addAddressResult;

            // Manipulations are commited and return success result if nothing failed
            _repository.Commit();
            return Result.Ok();
        }
    }
}
