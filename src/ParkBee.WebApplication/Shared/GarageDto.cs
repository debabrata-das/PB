using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("ParkBee.UnitTests")]
namespace ParkBee.WebApplication.Shared
{
    public class GarageDto
    {
        public Guid Identifier { get; set; }
        public string Name { get; set; }
        public AddressDto Address { get; set; }
        public OwnerDto Owner { get; set; }
        public IList<DoorDto> Doors { get; set; }
    }
}