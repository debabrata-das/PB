using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using ParkBee.Domain.AggregatesModel;
using ParkBee.WebApplication.Server.Exceptions;
using ParkBee.WebApplication.Server.Validators;
using ParkBee.WebApplication.Shared;

namespace ParkBee.WebApplication.Server.Commands
{
    public class SaveDoorCommandHandler : IRequestHandler<SaveDoorCommand, DoorDto>
    {
        private readonly IDoorRepository _doorRepository;
        private readonly IGarageRepository _garageRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public SaveDoorCommandHandler(IDoorRepository doorRepository, IGarageRepository garageRepository, IMapper mapper, ILoggerManager loggerManager)
        {
            _doorRepository = doorRepository;
            _garageRepository = garageRepository;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        public async Task<DoorDto> Handle(SaveDoorCommand request, CancellationToken cancellationToken)
        {
            List<ValidationException> errors = await ValidateModel(request);
            if (errors.Any())
            {
                throw new DataValidationException("Door validation failed", new AggregateException(errors));
            }

            try
            {
                return await SaveDoor(request, cancellationToken);
            }
            catch (Exception exc)
            {
                _loggerManager.Error($"Exception while creating Door! {Environment.NewLine}{exc}");
                throw;
            }
        }

        private async Task<DoorDto> SaveDoor(SaveDoorCommand request, CancellationToken cancellationToken)
        {
            _loggerManager.Info($"Trying to save Door {request.Identifier} for Garage {request.GarageIdentifier}");
            var doorToCreate = _mapper.Map<Door>(request);

            var garage = await _garageRepository.GetByIdentifier(doorToCreate.GarageIdentifier, cancellationToken);
            if (garage == null)
            {
                throw new DataValidationException($"Cannot create Door since the Garage with Identifier '{doorToCreate.GarageIdentifier}' not found", null);
            }

            var existingDoor = await _doorRepository.GetByIdentifier(doorToCreate.Identifier);
            if (existingDoor != null)
            {
                throw new CannotAddDuplicateEntityException("Door", request.Identifier);
            }

            await _doorRepository.Add(doorToCreate);
            await _doorRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            doorToCreate.Garage = garage;
            var dtoOut = _mapper.Map<Door, DoorDto>(doorToCreate);

            return dtoOut;
        }

        private static async Task<List<ValidationException>> ValidateModel(SaveDoorCommand request)
        {
            var validator = new DoorDtoValidator();
            var validationResult = await validator.ValidateAsync(request);
            List<ValidationException> validationErrors = new List<ValidationException>();
            if (!validationResult.IsValid)
            {
                validationErrors.AddRange(validationResult.Errors.Select(failure => new ValidationException(failure.ErrorMessage)));
            }

            return validationErrors;
        }
    }
}
