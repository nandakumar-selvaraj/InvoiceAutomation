using InvoiceAutomationApi.Models.Entities;

namespace InvoiceAutomationApi.Repositories
{
    public interface IInvoiceRepository
    {
        Task<Invoice?> GetByInvoiceNumberAsync(string invoiceNumber);
        Task<IEnumerable<Invoice>> GetAllAsync();
        Task AddAsync(Invoice invoice);
        void Update(Invoice invoice);
        void Delete(Invoice invoice);
        void DeleteItemsRange(IEnumerable<InvoiceDetail> items);
        Task<bool> SaveChangesAsync();
    }
}
