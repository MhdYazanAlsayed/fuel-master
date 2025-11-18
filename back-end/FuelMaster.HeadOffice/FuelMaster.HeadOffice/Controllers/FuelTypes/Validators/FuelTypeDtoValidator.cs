using FluentValidation;
using FuelMaster.HeadOffice.Core.Contracts.Repositories.FuelTypes.Dtos;
using FuelMaster.HeadOffice.Core.Resources;

namespace FuelMaster.HeadOffice.Controllers.FuelTypes.Validators
{
    public class FuelTypeDtoValidator : AbstractValidator<FuelTypeDto>
    {
        public FuelTypeDtoValidator()
        {
            RuleFor(x => x.ArabicName)
                .NotEmpty().WithMessage(Resource.ArabicNameRequired)
                .MaximumLength(100).WithMessage(Resource.ArabicNameMaxLength);

            RuleFor(x => x.EnglishName)
                .NotEmpty().WithMessage(Resource.EnglishNameMaxLength)
                .MaximumLength(100).WithMessage(Resource.EnglishNameMaxLength);
        }
    }
}

