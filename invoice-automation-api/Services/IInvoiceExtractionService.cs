using InvoiceAutomationApi.Models.Dtos;

namespace InvoiceAutomationApi.Services
{
    public interface IInvoiceExtractionService
    {
        Task<InvoiceResponse> ExtractInvoiceAsync(IFormFile file);
    }
}
