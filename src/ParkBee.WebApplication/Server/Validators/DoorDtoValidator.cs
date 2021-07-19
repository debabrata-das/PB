using FluentValidation;
using ParkBee.WebApplication.Shared;

namespace ParkBee.WebApplication.Server.Validators
{
    public class DoorDtoValidator : AbstractValidator<DoorDto>
    {
        public DoorDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("The name can not be null or empty");

            RuleFor(dto => dto.Identifier)
                .NotNull()
                .NotEmpty()
                .WithMessage("The Identifier value can not be null or empty");

            RuleFor(dto => dto.IpAddress)
                .NotNull()
                .NotEmpty()
                .WithMessage("The IpAddress value can not be null or empty");
        }
    }
}