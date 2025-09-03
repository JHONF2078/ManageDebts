using FluentValidation;
using ManageDebts.Domain.Constants;

namespace ManageDebts.Application.Debts.Commands
{
    public class EditDebtCommandValidator : AbstractValidator<EditDebtCommand>
    {
        public EditDebtCommandValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(DebtRules.MinDebtAmount)
                .WithMessage($"El monto debe ser mayor a {DebtRules.MinDebtAmount}.");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La descripción es obligatoria.");
        }
    }
}
