using InvoiceAutomationApi.Models.Entities;
using InvoiceAutomationApi.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAutomationApi.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly InvoiceAutomationDbContext _context;

        public InvoiceRepository(InvoiceAutomationDbContext context) => _context = context;

        public async Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
        }

        public async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .ToListAsync();
        }

        public async Task AddAsync(Invoice invoice) => await _context.Invoices.AddAsync(invoice);

        public void Update(Invoice invoice) => _context.Invoices.Update(invoice);

        public void Delete(Invoice invoice) => _context.Invoices.Remove(invoice);

        public void DeleteItemsRange(IEnumerable<InvoiceDetail> items) => _context.InvoiceDetails.RemoveRange(items);

        public async Task<bool> SaveChangesAsync() => await _context.SaveChangesAsync() > 0;
    }
}
