using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ParkBee.UnitTests")]
namespace ParkBee.WebApplication.Shared
{
    public class DoorDto
    {
        public Guid Identifier { get; set; }
        public Guid GarageIdentifier { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public DoorStatusDto DoorStatus { get; set; }
    }
}