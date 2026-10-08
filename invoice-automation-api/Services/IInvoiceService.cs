using InvoiceAutomationApi.Models.Dtos;

namespace InvoiceAutomationApi.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceResponse?> GetInvoiceAsync(string invoiceNumber);
        Task<IEnumerable<InvoiceResponse>> GetAllInvoicesAsync();
        Task<InvoiceResponse> CreateInvoiceAsync(InvoiceResponse dto);
        Task<bool> UpdateInvoiceAsync(string invoiceNumber, InvoiceResponse dto);
        Task<bool> DeleteInvoiceAsync(string invoiceNumber);
        Task<IEnumerable<InvoiceResponse>> CreateInvoicesBulkAsync(IEnumerable<InvoiceResponse> dtos);

        Task<byte[]> ExportInvoicesToExcelAsync();

    }
}
