using System.ComponentModel.DataAnnotations;

namespace ManageDebts.Application.Debts
{
    public class CreateDebtCommand
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string CreditorId { get; set; } = string.Empty; // Acreedor
    }

    public class EditDebtCommand
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string CreditorId { get; set; } = string.Empty; // Acreedor
    }
}
