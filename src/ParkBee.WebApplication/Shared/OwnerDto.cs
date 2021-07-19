using System;

namespace ParkBee.WebApplication.Shared
{
    public class OwnerDto
    {
        public Guid Identifier { get; set; }
        public Guid GarageIdentifier { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
