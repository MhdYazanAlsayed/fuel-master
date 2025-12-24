using FluentValidation;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Employees.DTOs;
using FuelMaster.HeadOffice.Core.Entities;

namespace FuelMaster.HeadOffice.Controllers.Employees.Validators;

public class UpdateEmployeeDtoValidator : AbstractValidator<UpdateEmployeeDto>
{
    public UpdateEmployeeDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required")
            .MaximumLength(200).WithMessage("Full name must not exceed 200 characters");

        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card number is required")
            .MaximumLength(50).WithMessage("Card number must not exceed 50 characters");

        RuleFor(x => x.RoleId)
            .GreaterThan(0).WithMessage("Role ID must be greater than 0");

        RuleFor(x => x.Scope)
            .IsInEnum().WithMessage("Scope must be a valid value");

        RuleFor(x => x.StationId)
            .GreaterThan(0).When(x => x.Scope == Scope.Station)
            .WithMessage("Station ID is required when scope is Station")
            .Null().When(x => x.Scope == Scope.Self)
            .WithMessage("Station ID cannot be provided when scope is Self");

        RuleFor(x => x.AreaId)
            .GreaterThan(0).When(x => x.Scope == Scope.Area)
            .WithMessage("Area ID is required when scope is Area");

        RuleFor(x => x.CityId)
            .GreaterThan(0).When(x => x.Scope == Scope.City)
            .WithMessage("City ID is required when scope is City");

        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(18).When(x => x.Age.HasValue)
            .WithMessage("Age must be greater than or equal to 18");

        RuleFor(x => x.EmailAddress)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.EmailAddress))
            .WithMessage("Email address must be a valid email format");
    }
}

