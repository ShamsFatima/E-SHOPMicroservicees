using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
    public record Address
    {
        public string FirstName { get; }=default!;
        public string LastName { get; } = default!;
        public string? EmailAddrress { get; } = default!;
        public string AddressLine { get; } = default!;  
        public string Country { get; } = default!;  
        public string State { get; } = default!;    
        
        public string ZipCode { get; } = default!;
        public Address()
        {

        }
        private Address(string firstName, string lastName, string emailAddress, string country, string state, string zipCode, string addressLine)
        {
            FirstName = firstName;
            LastName = lastName;
            EmailAddrress = emailAddress;
            Country = country;
            State = state;
            ZipCode = zipCode;
            AddressLine = addressLine;
        }
        public static Address Of(string firstName, string lastName, string emailAddress, string country, string state, string zipCode, string addressLine)
        {
            ArgumentNullException.ThrowIfNull(country, nameof(country));
            return new Address(firstName, lastName, emailAddress, country, state, zipCode, addressLine);
        }

    }
}
