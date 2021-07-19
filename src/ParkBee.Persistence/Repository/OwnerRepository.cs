using System;
using System.Threading.Tasks;
using ParkBee.Domain.AggregatesModel;
using ParkBee.Domain.SeedWork;

namespace ParkBee.Persistence.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly ApplicationDbContext _context;

        public OwnerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task AddOwner(Owner owner)
        {
            await _context.Owners.AddAsync(owner);
            await _context.SaveChangesAsync();
        }

        public async Task<Owner> GetByIdentifier(Guid identifier)
        {
            var owner = await _context.Owners.FindAsync(identifier);
            return owner;
        }

        public async Task RemoveOwner(Guid identifier)
        {
            var owner = await _context.Owners.FindAsync(identifier);
            if (owner == null)
            {
                return;
            }

            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();
        }
    }
}
