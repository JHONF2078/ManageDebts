namespace ManageDebts.Domain.Entities
{
    public class Debt
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = null!; // Deudor
        public string CreditorId { get; set; } = null!; // Acreedor
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsPaid { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
