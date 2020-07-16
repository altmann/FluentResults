using System;

namespace FluentResults.Samples.DomainDrivenDesign
{
    // Entity
    public class Customer
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public Address Address { get; private set; }

        private Customer()
        {
            Id = Guid.NewGuid();
        }

        // Return type of Result<Customer> is not necessary in this scenario because this method contains no validation
        public static Customer Create()
        {
            return new Customer();
        }

        // Return a Result object which indicates success or failure of the operation
        public Result AddAddress(Address address)
        {
            // Execute business rule and return failed result if not valid
            if (address.City != "New York City")
                return Result.Fail("Address of customer should be in New York City.");

            Address = address;

            // Return success result
            return Result.Ok();
        }
    }
}