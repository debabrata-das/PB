using System;
using System.Threading.Tasks;
using ParkBee.Domain.SeedWork;

namespace ParkBee.Domain.AggregatesModel
{
    public interface IOwnerRepository : IRepository<Owner>
    {
        Task AddOwner(Owner owner);
        Task<Owner> GetByIdentifier(Guid identifier);
        Task RemoveOwner(Guid identifier);
    }
}
