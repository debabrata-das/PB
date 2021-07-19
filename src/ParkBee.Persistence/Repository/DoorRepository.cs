using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkBee.Domain.AggregatesModel;
using ParkBee.Domain.SeedWork;

namespace ParkBee.Persistence.Repository
{
    public class DoorRepository : IDoorRepository
    {
        private readonly ApplicationDbContext _context;

        public IUnitOfWork UnitOfWork =>_context;

        public DoorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Door door)
        {
            await _context.Doors.AddAsync(door);
            await _context.SaveChangesAsync();
        }

        public async Task<Door> GetByIdentifier(Guid identifier)
        {
            return await _context.Doors.FirstOrDefaultAsync(cs => cs.Identifier == identifier);
        }

        public async Task RemoveDoor(Guid identifier)
        {
            var item = await _context.Doors.FindAsync(identifier);
            if (item != null)
            {
                await RemoveDoor(item);
            }
            await _context.SaveChangesAsync();
        }

        public async Task RemoveDoor(Door door)
        {
            _context.Doors.Remove(door);
            await _context.SaveChangesAsync();
        }

        public async Task<IList<Door>> GetByGarageIdentifier(Guid garageIdentifier)
        {
            return await _context.Doors.Include(g => g.Garage)
                .Where(door => door.GarageIdentifier == garageIdentifier).ToListAsync();
        }

        public async Task<Door> GetLatestDoorStatusForDoor(Door garageDoor, CancellationToken cancellationToken)
        {
            DoorStatus doorStatus = GetCurrentDoorStatus();

            if (garageDoor.Status != doorStatus)
            {
                return await PersistDoorStatus(garageDoor, doorStatus, cancellationToken);
            }

            return garageDoor;
        }


        public async Task<Door> PersistDoorStatus(Door door, DoorStatus newDoorStatus, CancellationToken cancellationToken)
        {
            DoorStatus oldDoorStatus = door.Status;

            var loadedDoor = await _context.Doors.FindAsync(door.Identifier, cancellationToken);
            loadedDoor.Status = newDoorStatus;

            await SaveDoorStatusToDoorStatusHistory(loadedDoor, oldDoorStatus, newDoorStatus, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return loadedDoor;
        }

        private async Task SaveDoorStatusToDoorStatusHistory(Door d, DoorStatus oldDoorStatus, DoorStatus newDoorStatus, CancellationToken cancellationToken)
        {
            var doorStatusHistory = new DoorStatusHistory
            {
                DoorIdentifier = d.Identifier,
                NewStatus = newDoorStatus,
                OldStatus = oldDoorStatus,
                ModifiedOn = DateTime.UtcNow
            };

            await _context.DoorStatusHistory.AddAsync(doorStatusHistory, cancellationToken);
        }

        public DoorStatus GetCurrentDoorStatus()
        {
            Array values = Enum.GetValues(typeof(DoorStatus));
            DoorStatus doorStatus = (DoorStatus)new Random().Next(0, values.Length - 1);
            return doorStatus;
        }
    }
}
