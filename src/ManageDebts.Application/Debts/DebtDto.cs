using System;

namespace ManageDebts.Application.Debts
{
    public class DebtDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!;
        public string DebtorName { get; set; } = string.Empty;
        public string CreditorId { get; set; } = null!;
        public string CreditorName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
