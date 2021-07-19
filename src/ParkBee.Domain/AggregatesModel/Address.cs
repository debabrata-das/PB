using System;
using ParkBee.Domain.SeedWork;

namespace ParkBee.Domain.AggregatesModel
{
    public class Address : Entity, IAggregateRoot
    {
        private Garage _garage;
        private Guid _identifier;

        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }

        public Guid GarageIdentifier { get; set; }

        public Guid Identifier
        {
            get => _identifier;
            set
            {

                _identifier = value;
                Id = Identifier;
            }
        }

        public virtual Garage Garage
        {
            get => _garage;
            set
            {
                _garage = value;
                if (_garage != null)
                {
                    GarageIdentifier = _garage.Id;
                }
            }
        }

        public Address() { }
    }
}
