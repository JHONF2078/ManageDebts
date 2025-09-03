using FluentValidation;
using ManageDebts.Application.Auth.Register.Commands;


namespace ManageDebts.Application.Auth.Register
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);          
        }
    }
}
