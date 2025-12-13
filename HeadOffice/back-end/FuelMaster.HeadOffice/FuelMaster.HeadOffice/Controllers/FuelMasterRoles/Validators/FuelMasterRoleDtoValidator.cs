using FluentValidation;
using FuelMaster.HeadOffice.Application.Services.Implementations.Business.FuelMasterRoleService.DTOs;
using FuelMaster.HeadOffice.Core.Resources;

namespace FuelMaster.HeadOffice.Controllers.FuelMasterRoles.Validators;

public class FuelMasterRoleDtoValidator : AbstractValidator<FuelMasterRoleDto>
{
    public FuelMasterRoleDtoValidator()
    {
        RuleFor(x => x.ArabicName)
            .NotEmpty().WithMessage(Resource.ArabicNameRequired)
            .MaximumLength(100).WithMessage(Resource.ArabicNameMaxLength);

        RuleFor(x => x.EnglishName)
            .NotEmpty().WithMessage(Resource.EnglishNameMaxLength)
            .MaximumLength(100).WithMessage(Resource.EnglishNameMaxLength);

        RuleFor(x => x.AreasOfAccessIds)
            .NotEmpty().WithMessage("Areas of access are required")
            .Must(areas => areas != null && areas.Count > 0).WithMessage("At least one area of access is required");

        RuleForEach(x => x.AreasOfAccessIds)
            .NotEmpty()
            .WithMessage("");
    }
}

