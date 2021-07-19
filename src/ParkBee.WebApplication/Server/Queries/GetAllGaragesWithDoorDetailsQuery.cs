using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ParkBee.Domain.AggregatesModel;
using ParkBee.WebApplication.Shared;

namespace ParkBee.WebApplication.Server.Queries
{
    public class GetAllGaragesWithDoorDetailsQuery : IRequest<IList<GarageDto>> { }

    /// <summary>
    /// 
    /// </summary>
    public class GetAllGaragesWithDoorDetailsQueryHandler
        : IRequestHandler<GetAllGaragesWithDoorDetailsQuery, IList<GarageDto>>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IGarageRepository _garageRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public GetAllGaragesWithDoorDetailsQueryHandler(IDoorRepository doorRepository, IGarageRepository garageRepository, IMapper mapper, ILoggerManager loggerManager)
        {
            _doorRepository = doorRepository;
            _garageRepository = garageRepository;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        public async Task<IList<GarageDto>> Handle(GetAllGaragesWithDoorDetailsQuery request, CancellationToken cancellationToken)
        {
            IList<GarageDto> list = new List<GarageDto>();

            IList<Guid> ids = await _garageRepository.GetAllGarageIdentifiers(cancellationToken);
            _loggerManager.Debug($"Found {ids.Count} Garages from repository");
            foreach (var garageId in ids)
            {
                var garage = await _garageRepository.GetByIdentifier(garageId, cancellationToken);
                foreach (var garageDoor in garage.Doors)
                {
                    await _doorRepository.GetLatestDoorStatusForDoor(garageDoor, cancellationToken);
                }

                var garageDto = _mapper.Map<Garage, GarageDto>(garage);
                list.Add(garageDto);
            }

            return list;
        }
    }
}
