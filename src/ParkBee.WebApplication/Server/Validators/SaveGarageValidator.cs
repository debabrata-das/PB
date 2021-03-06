using FluentValidation;
using ParkBee.WebApplication.Server.Commands;

namespace ParkBee.WebApplication.Server.Validators
{
    public class SaveGarageValidator : AbstractValidator<SaveGarageCommand>
    {
        public SaveGarageValidator()
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