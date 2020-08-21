using System;

namespace FluentResults.Samples.DomainDrivenDesign
{
    public class CreateAddressCommand
    {
        public Guid CustomerId { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string City { get; set; }
    }
}