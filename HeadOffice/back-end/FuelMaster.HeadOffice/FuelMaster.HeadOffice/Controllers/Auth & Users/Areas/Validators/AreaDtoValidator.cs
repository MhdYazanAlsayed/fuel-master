using FluentValidation;
using FuelMaster.HeadOffice.Application.Services.Interfaces.Business.Areas.DTOs;
using FuelMaster.HeadOffice.Core.Resources;

namespace FuelMaster.HeadOffice.Controllers.Areas.Validators;

public class AreaDtoValidator : AbstractValidator<AreaDto>
{
    public AreaDtoValidator()
    {
        RuleFor(x => x.ArabicName)
            .NotEmpty().WithMessage(Resource.ArabicNameRequired)
            .MaximumLength(100).WithMessage(Resource.ArabicNameMaxLength);

        RuleFor(x => x.EnglishName)
            .NotEmpty().WithMessage(Resource.EnglishNameMaxLength)
            .MaximumLength(100).WithMessage(Resource.EnglishNameMaxLength);
    }
}

