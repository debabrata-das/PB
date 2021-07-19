using System;
using ParkBee.Domain.SeedWork;

namespace ParkBee.Domain.AggregatesModel
{
    public class Owner : Entity, IAggregateRoot
    {
        private Garage _garage;
        private Guid _identifier; 
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

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
    }
}
