using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MediatR;
using ParkBee.Domain.AggregatesModel;
using ParkBee.WebApplication.Server.Exceptions;
using ParkBee.WebApplication.Server.Validators;
using ParkBee.WebApplication.Shared;

namespace ParkBee.WebApplication.Server.Commands
{
    public class SaveGarageCommandHandler : IRequestHandler<SaveGarageCommand, GarageDto>
    {
        private readonly IGarageRepository _garageRepository;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public SaveGarageCommandHandler(IGarageRepository garageRepository, IMapper mapper, ILoggerManager loggerManager)
        {
            _garageRepository = garageRepository;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        public async Task<GarageDto> Handle(SaveGarageCommand request, CancellationToken cancellationToken)
        {
            List<ValidationException> errors = await ValidateModel(request);
            if (errors.Any())
            {
                throw new DataValidationException("Garage validation failed", new AggregateException(errors));
            }

            try
            {
                var newGarageDto = await SaveGarage(request, cancellationToken);

                return newGarageDto;
            }
            catch (Exception exc)
            {
                _loggerManager.Error($"Exception while creating Garage! \r\n{exc}");
                throw;
            }
        }

        private async Task<GarageDto> SaveGarage(SaveGarageCommand request, CancellationToken cancellationToken)
        {
            var garageToCreate = _mapper.Map<Garage>(request);
            _loggerManager.Info($"Trying to save Garage with identifier '{request.Identifier}' with name '{request.Name}'");

            var existingGarage = await _garageRepository.GetByIdentifier(garageToCreate.Identifier, cancellationToken);
            if (existingGarage != null)
            {
                throw new CannotAddDuplicateEntityException("Garage", request.Identifier);
            }

            await _garageRepository.AddGarage(garageToCreate, cancellationToken);
            await _garageRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

            var newGarageDto = _mapper.Map<Garage, GarageDto>(garageToCreate);
            return newGarageDto;
        }

        private static async Task<List<ValidationException>> ValidateModel(GarageDto garageDto)
        {
            var validator = new GarageDtoValidator();
            var validationResult = await validator.ValidateAsync(garageDto);
            List<ValidationException> validationErrors = new List<ValidationException>();
            if (!validationResult.IsValid)
            {
                validationErrors.AddRange(validationResult.Errors.Select(failure => new ValidationException(failure.ErrorMessage)));
            }

            return validationErrors;
        }
    }
}
