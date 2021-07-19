using System;

namespace ParkBee.WebApplication.Shared
{
    public class AddressDto
    {
        public Guid Identifier { get; set; }
        public Guid GarageIdentifier { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }
}
