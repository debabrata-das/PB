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
    public class GarageRepository : IGarageRepository
    {
        private readonly ApplicationDbContext _context;

        public GarageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task AddGarage(Garage garage, CancellationToken cancellationToken)
        {
            await _context.Garages.AddAsync(garage, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Garage> GetByIdentifier(Guid identifier, CancellationToken cancellationToken)
        {
            var garage = await _context.Garages.FindAsync(identifier, cancellationToken);
            if (garage != null)
            {
                return _context.Garages.Include(g => g.Doors)
                    .ThenInclude(door => door.Garage)
                    .Include(g => g.Address)
                    .ThenInclude(address => address.Garage)
                    .Include(g => g.Owner)
                    .ThenInclude(owner => owner.Garage)
                    .FirstOrDefault(g => g.Identifier == identifier);
            }

            return null;
        }

        

        public async Task RemoveGarage(Guid identifier, CancellationToken cancellationToken)
        {
            var garage = await _context.Garages.FindAsync(identifier, cancellationToken);
            if (garage == null)
            {
                return;
            }

            _context.Garages.Remove(garage);
            var doors = _context.Doors.Where(c => c.GarageIdentifier == identifier);
            foreach (var door in doors)
            {
                _context.Doors.Remove(door);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IList<Guid>> GetAllGarageIdentifiers(CancellationToken cancellationToken)
        {
            return await _context.Set<Garage>().Select(x=> x.Identifier).ToListAsync(cancellationToken);
        }
    }
}
