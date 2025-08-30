using ManageDebts.Domain.Entities;
using ManageDebts.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ManageDebts.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder b)
        {
            base.OnModelCreating(b);
            // configuraciones extra si tienes entidades de dominio
            // b.HasDefaultSchema("public"); // por defecto
        }

        public DbSet<Debt> Debts { get; set; }
    }
}
