using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ParkBee.Domain.AggregatesModel;
using ParkBee.WebApplication.Shared;

namespace ParkBee.WebApplication.Server.Queries
{
    public class GetGarageDetailsQuery : GarageDto, IRequest<GarageDto> { }

    public class GetGarageDetailsQueryHandler : IRequestHandler<GetGarageDetailsQuery, GarageDto>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IGarageRepository _garageRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public GetGarageDetailsQueryHandler(IDoorRepository doorRepository, IGarageRepository garageRepository, IMapper mapper, ILoggerManager loggerManager)
        {
            _doorRepository = doorRepository;
            _garageRepository = garageRepository;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        public async Task<GarageDto> Handle(GetGarageDetailsQuery request, CancellationToken cancellationToken)
        {
            var garageToLookup = _mapper.Map<Garage>(request);

            var garage = await _garageRepository.GetByIdentifier(garageToLookup.Identifier, cancellationToken);
            if (garage == null)
            {
                _loggerManager.Debug($"No Garage found with identifier - {garageToLookup.Identifier}");
                return null;
            }
            else
            {
                _loggerManager.Debug($"Found Garage with identifier - {garageToLookup.Identifier}");
                var garageResource = _mapper.Map<Garage, GarageDto>(garage);
                return garageResource;
            }
        }
    }
}