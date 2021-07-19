using System;
using System.Collections.Generic;
using ParkBee.Domain.SeedWork;

namespace ParkBee.Domain.AggregatesModel
{
    public class Garage: Entity, IAggregateRoot
    {
        private Guid _identifier;
        
        public Guid Identifier
        {
            get => _identifier;
            set
            {
                _identifier = value;
                Id = Identifier;
            }
        }

        public string Name { get; set; }
        
        public Address Address { get; set; }

        public Owner Owner { get; set; }
        
        public HashSet<Door> Doors { get; set; }

        public Garage()
        {
            Doors = new HashSet<Door>(new DoorComparer());
        }
    }
}
