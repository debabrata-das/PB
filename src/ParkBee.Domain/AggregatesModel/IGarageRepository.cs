using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ParkBee.Domain.SeedWork;

namespace ParkBee.Domain.AggregatesModel
{
    public interface IGarageRepository : IRepository<Garage>
    {
        Task AddGarage(Garage garage, CancellationToken cancellationToken);
        Task<Garage> GetByIdentifier(Guid identifier, CancellationToken cancellationToken);
        Task RemoveGarage(Guid identifier, CancellationToken cancellationToken);
        Task<IList<Guid>> GetAllGarageIdentifiers(CancellationToken cancellationToken);
    }
}
