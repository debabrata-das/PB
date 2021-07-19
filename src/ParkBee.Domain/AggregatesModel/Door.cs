using System;
using System.Collections.Generic;
using ParkBee.Domain.SeedWork;

namespace ParkBee.Domain.AggregatesModel
{
    public class Door : Entity, IAggregateRoot
    {
        private Garage _garage;
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

        public Guid GarageIdentifier { get; set; }

        public string Name { get; set; }
        
        public DoorStatus Status { get; set; }

        public string IpAddress { get; set; }

        public Garage Garage
        {
            get => _garage;
            set
            {
                _garage = value;
                if (_garage != null)
                {
                    GarageIdentifier = _garage.Identifier;
                }
            }
        }

        public Door() { }
    }

    public class DoorComparer : IEqualityComparer<Door>
    {
        public bool Equals(Door door, Door otherDoor)
        {
            if (door == null && otherDoor == null)
            {
                return true;
            }

            if (door == null || otherDoor == null)
            {
                return false;
            }

            return door.Identifier.Equals(otherDoor.Identifier) || door.IpAddress.Equals(otherDoor.IpAddress);
        }

        public int GetHashCode(Door door)
        {
            return door.Identifier.GetHashCode() ^ door.IpAddress.GetHashCode() ^ door.Name.GetHashCode();
        }
    }
}

