using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ParkBee.Domain.SeedWork;

namespace ParkBee.Domain.AggregatesModel
{
    public interface IDoorRepository : IRepository<Door>
    {
        Task Add(Door door);
        Task<Door> GetByIdentifier(Guid identifier);
        Task RemoveDoor(Guid identifier);
        Task RemoveDoor(Door door);
        Task<IList<Door>> GetByGarageIdentifier(Guid garageIdentifier);
        Task<Door> GetLatestDoorStatusForDoor(Door garageDoor, CancellationToken cancellationToken);
    }
}
