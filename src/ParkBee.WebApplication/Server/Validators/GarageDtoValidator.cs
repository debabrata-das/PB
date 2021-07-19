using FluentValidation;
using ParkBee.WebApplication.Shared;

namespace ParkBee.WebApplication.Server.Validators
{
    public class GarageDtoValidator : AbstractValidator<GarageDto>
    {
        public GarageDtoValidator()
        {
            RuleFor(dto => dto.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("The Name value can not be null or empty");

            RuleFor(dto => dto.Identifier)
                .NotNull()
                .NotEmpty()
                .WithMessage("The Identifier value can not be null or empty");
        }
    }
}