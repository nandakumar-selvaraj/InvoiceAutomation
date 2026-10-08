using InvoiceAutomationApi.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAutomationApi.Repositories.Context
{
    public class InvoiceAutomationDbContext : DbContext
    {
        public InvoiceAutomationDbContext(DbContextOptions<InvoiceAutomationDbContext> options) : base(options)
        {
        }

        // These tell EF Core to build tables for your specific Entities
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Sets up the relationship configuration cleanly
            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.Items)
                .WithOne(item => item.Invoice)
                .HasForeignKey(item => item.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade); // If an invoice is deleted, its items are deleted automatically
        }
    }
}
