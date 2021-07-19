using System;
using ParkBee.Domain.SeedWork;

namespace ParkBee.Domain.AggregatesModel
{
    public class DoorStatusHistory : Entity, IAggregateRoot
    {
        public Guid DoorIdentifier { get; set; }
        public DoorStatus OldStatus { get; set; }
        public DoorStatus NewStatus { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
