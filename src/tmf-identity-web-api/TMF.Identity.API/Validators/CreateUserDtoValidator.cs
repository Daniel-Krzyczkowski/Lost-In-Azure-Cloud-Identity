using FluentValidation;
using TMF.Identity.API.Dto;

namespace TMF.Identity.API.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(model => model.Email)
                .EmailAddress()
                .WithMessage("Email property is required for the new user");
        }
    }
}
